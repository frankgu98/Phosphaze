using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Phosphaze.Core
{
    //MapMaker class that makes .dml maps for songs
    public class MapMaker
    {
        Wave wave;//wav file wrapper
        Audio wavAudio;//audio player wrapper
        StatBeatDetection SBD;//beat detector


        SoundInfo[] soundInformation;//sound information at points in the song

        //possible spawn locations
        DirectedSpawnLocation[] spawnLocations = {new DirectedSpawnLocation(new Vector2(Options.Resolutions.X / 4, 0),45,120),
                                                        new DirectedSpawnLocation(new Vector2(Options.Resolutions.X / 3, 0),45,120),
                                                        new DirectedSpawnLocation(new Vector2(Options.Resolutions.X / 2, 0),45,135),
                                                        new DirectedSpawnLocation(new Vector2(2*Options.Resolutions.X / 3, 0),60,135), 
                                                        new DirectedSpawnLocation(new Vector2(3*Options.Resolutions.X / 4, 0),60,135)
                                                        };

        const double scale = 96;//multiplier between index in soundinformation list to correct time in ms for the song

        //constructor takes in a Wave object which is used to initialize the fields
        public MapMaker(Wave w)
        {
            wave = w;
            wavAudio = new Audio(wave, 1000000);//1000000

            SBD = new StatBeatDetection(wave, 1000000);
            
            soundInformation = new SoundInfo[SBD.length() + 1];
        }

        //makes the dml map for the Wave
        public string MakeMap()
        {
            string map="";//starts as nothing
            memblock block;//block memory containing sound information (or pointers to it)

            //goes through the Wave and keeps track of energy levels at points in time
            for (int i = 0; i < soundInformation.Length; i++)
            {
                block = wave.next(wavAudio.framesAvailable());//looks at the next memblock in the Wave
                soundInformation[i] = new SoundInfo(false, Math.Abs(wave.getStereoObject(block).getAvg()));//saves the energy level
            }

            wave.reset();//resets the wave (so you're looking at the first memblock again)

            //goes through the SBD object and keeps track of if there is a beat at each point in time
            for (int i = 0; i < soundInformation.Length; i++)
            {
                //if statement needed since the lengths of SBD and wave are always 1 or 2 off from eachother
                if (SBD.hasNext())
                {
                    soundInformation[i].isBeat = SBD.next() != 0;
                }                
                else
                {
                    soundInformation[i].isBeat = false;
                }
            }

            SBD.release();
            wave.reset();

            //defines the 2 bullets that will be used
            map+=DefineGenericBullet("Standard", "bullets/circle10r12g0080ff", 90, 5);
            map += DefineGenericBullet("HighLeeway", "bullets/circle10r12g0080ff", 90, 5, 40);
            //begins the dml timeline
            string timeline = "Timeline <";

            //starts the update for left and right generator bullets in the top corners of the screen
            //they are only there to BurstSpawn since it's not implemented in the timeline
            string leftGenUpdate = "";
            string rightGenUpdate = "";

            int waveSize = 0;//number of bullets that would be spawned in a wave
            int consecutiveBeatCount = 0;//consecutives beats after this point
            int curSweepCount = 0;//how many sweeps are currently spawning
            int curLineCount = 0;//how many lines are currently spawning
            int countReleaseTime = 0;//when the previous 2 counts can be set back to 0
            int burstBulCount = 0;//minimum number of bullets that would be spawned in a burst

            DirectedSpawnLocation curDSL;//current spawn location for a specific pattern

            //spawns the LeftGen and RightGen bullets
            timeline += "\r\n";
            timeline += SpawnBullet(1, 0, "LeftGen", new Vector2(-10, 0));
            timeline += "\r\n";
            timeline += SpawnBullet(1, 0, "RightGen", new Vector2(Options.Resolutions.X + 10, 0));

            //note that every spot represents approximately 1/10 of a second
            //goes through each piece of sound information in the song
            for (int i = 0; i < soundInformation.Count(); i++)
            {
                //checks if this point in time is a beat
                if (soundInformation[i].isBeat)
                {
                    //
                    if (i == countReleaseTime)
                    {
                        curSweepCount = 0;
                        curLineCount = 0;
                    }

                    //checks if the next few times are also beats, if so, spawn a wave or a line of bullets instead of the regular mishmash
                    for (int j = 0; j < 15; j++)
                    {
                        if (i + j < soundInformation.Count() && soundInformation[i + j].isBeat)
                        {
                            consecutiveBeatCount = j;//so i don't have to reset it
                        }
                        else
                        {
                            break;
                        }
                    }

                    //if there are some consecutive beats
                    if (consecutiveBeatCount > 5)
                    {
                        countReleaseTime = i + consecutiveBeatCount;//sets the time when the lines and sweeps won't be spawning
                        curDSL = spawnLocations[Globals.randGen.Next(0, 5)];//chooses a location to spawn the line/sweep

                        //1/3 chance of spawning each time
                        //random generator doesn't really stop a line from spawning, it just makes it more likely spawn later since consecutivebeatcount is only 1 lower from
                        //the next spot in soundinformation, which is usually over 5
                        //only 3 lines can be spawning at once
                        if (Globals.randGen.Next(0, 3) == 0 && curLineCount < 3)
                        {
                            curLineCount++;
                            timeline += SpawnLine(1,//1 indent in
                                                  i * scale,//start time converted to ms
                                                  (i + consecutiveBeatCount) * scale,//end time converted to ms
                                                  consecutiveBeatCount,//1 bullet for each beat
                                                  "Standard",//standard bullets
                                                  curDSL.location,//spawning from a prechosen location
                                                  Globals.randGen.Next(curDSL.minAngle, curDSL.maxAngle + 1),//randomly chosen angle between 2 bounds based on location
                                                  soundInformation[i].speed);//the pregenerated speed for that time
                        }

                        //1/3 chance of spawning each time
                        //only 3 sweeps can be spawning at once
                        if (Globals.randGen.Next(0, 3) == 0 && curSweepCount < 3)
                        {
                            curSweepCount++;
                            //direction the sweep sweeps is chosen based on position
                            if (curDSL.location.X < Options.Resolutions.X / 2)
                            {
                                timeline += SpawnSweep(1,//1 indent in
                                                       i * scale,//start time converted to ms
                                                       (i + consecutiveBeatCount) * scale,//end time converted to ms
                                                       consecutiveBeatCount * 2,//2 bullets for each beat
                                                       "Standard",//standard bullets
                                                       curDSL.location,//spawning from a prechosen location
                                                       30,150,//sweeps sweep across the map so they don't use the normal bounds for their spawn location 
                                                       -1,//if it's spawning from the left, it goes counterclockwise
                                                       soundInformation[i].speed);//the pregenerated speed for that time
                            }

                            else if (curDSL.location.X == Options.Resolutions.X / 2)
                            {
                                timeline += SpawnSweep(1,
                                                 i * scale,
                                                 (i + consecutiveBeatCount) * scale,
                                                 consecutiveBeatCount * 2,
                                                 "Standard",
                                                 curDSL.location,
                                                 30,
                                                 150,
                                                 2 * Globals.randGen.Next(0, 2) - 1,//if it's spawning from the middle, it goes a random direction
                                                 soundInformation[i].speed);
                            }
                            else if (curDSL.location.X > Options.Resolutions.X / 2)
                            {
                                timeline += SpawnSweep(1,
                                                 i * scale,
                                                 (i + consecutiveBeatCount) * scale,
                                                 consecutiveBeatCount * 2,
                                                 "Standard",
                                                 curDSL.location,
                                                 30,
                                                 150,
                                                 1,//if it's spawning from the right, it goes clockwise
                                                 soundInformation[i].speed);
                            }
                        }
                    }

                    timeline += "\r\n";

                    //1/2 chance to spawn a wave
                    if (Globals.randGen.Next(0, 2) == 0)
                    {
                        //calculates what the wavesize would be before spawning
                        waveSize = (int)(soundInformation[i].energyLevel / 5000) + 3;
                        if (waveSize > 5)
                        {
                            curDSL = spawnLocations[Globals.randGen.Next(0, 5)];
                            timeline += SpawnWave(1,
                                                  i * scale,
                                                  waveSize,
                                                  "HighLeeway",//needs a higher leeway since some bullets may start outside the regular leeway
                                                  (int)(soundInformation[i].energyLevel / 500) + 50,//spawnpoint distance from origin spawn point decided by energylevel+constant
                                                  curDSL.location - new Vector2(0, 50),//origin is 50 units up from the regular spawnpoints 
                                                  Globals.randGen.Next(curDSL.minAngle, curDSL.maxAngle + 1),//kept within the normal angle bounds
                                                  soundInformation[i].speed - 1.5);//wave is meant to be slower than normal bullets
                        }
                    }

                    //if this point in time is significantly louder than the last second
                    if (0 <= i - 10 && soundInformation[i].energyLevel > 4 * AverageEnergyInRange(soundInformation, i - 10, i))
                    {
                        //calculates the minimum amount of bullets in the burst would be
                        burstBulCount = (int)(soundInformation[i].energyLevel / 3000);
                        if (burstBulCount > 5)
                        {
                            //writes the burst to the Update of the generator bullets
                            //both bursts happen at the same time
                            leftGenUpdate += "\r\n";
                            leftGenUpdate += SpawnBurst(2,
                                                        i * scale,
                                                        burstBulCount,
                                                        "Standard",
                                                        20, 60,
                                                        burstBulCount, burstBulCount + 4);//the speed of the bullets is also aproximately how many there are expected to be
                            rightGenUpdate += "\r\n";
                            rightGenUpdate += SpawnBurst(2,
                                                        i * scale,
                                                        burstBulCount,
                                                        "Standard",
                                                        120, 160,
                                                        burstBulCount, burstBulCount + 4);
                        }

                    }

                    timeline += "\r\n";

                    //it will always spawn a ring when there's a beat
                    curDSL = spawnLocations[Globals.randGen.Next(0, 5)]; //where it's spawned is decided at random
                    timeline += SpawnRing(1,
                                          i * scale,
                                          Globals.randGen.Next(8, 12) + (int)(soundInformation[i].energyLevel / 1000), //how many spawned decided by energy level and some randomness
                                          "Standard",
                                          curDSL.location, 
                                          soundInformation[i].speed,
                                          Globals.randGen.Next(-30, 31)); //its shift is also random



                }
            }
            timeline += "\r\n>";
            //adds the definitions for the  generator bullets
            map+=String.Format(DefineGeneratorBullet("LeftGen"), leftGenUpdate);
            map+="\r\n";
            map+=String.Format(DefineGeneratorBullet("RightGen"), rightGenUpdate);
            map+="\r\n";
            //adds the timeline
            map+=timeline;

            
            return map;

        }

        //returns a string of a bullet which will go in a straight line
        private string DefineGenericBullet(string name, string spritename, double? direction = null, double? speed = null, double? leeway=null)
        {
            string ans = "";

            ans += String.Format("Bullet @{0} <", name);
            ans += "\r\n";
            ans += Indentation(1) + "Init <";
            ans += "\r\n";
            ans += Indentation(2) + String.Format("Assign $Sprite \"{0}\";", spritename);
            if (direction.HasValue)
            {
                ans += "\r\n";
                ans += Indentation(2) + String.Format("Assign $Direction PolarD({0});", direction);
            }
            if (speed.HasValue)
            {
                ans += "\r\n";
                ans += Indentation(2) + String.Format("Assign $Speed {0};", speed);
            }

            ans += "\r\n" + Indentation(1) + ">";
            ans += "\r\n";
            ans += Indentation(1) + "Update <";
            ans += "\r\n";
            //unlike the other nullable doubles, leeway is really important and hard to define anywhere else so it'll always be defined
            ans += Indentation(2) + String.Format("KillIfOffscreen | %Leeway {0};",leeway.HasValue?leeway:10);
            ans += "\r\n" + Indentation(1) + ">"; ;
            ans += "\r\n>";
            ans += "\r\n";
            return ans;
        }

        //returns a string of a generator bullet which leaves a spot to put in your own definistion for the update 
        private string DefineGeneratorBullet(string name)
        {
            string ans = "";
            ans += String.Format("Bullet @{0} <", name);
            ans += "\r\n";
            ans += Indentation(1) + "Update <";
            ans += "{0}";
            ans += "\r\n";
            ans += Indentation(1) + ">";
            ans += "\r\n";
            ans += ">";
            return ans;
        }

        //spawns a bullet of a given type
        private string SpawnBullet(string bulletType, Vector2? origin = null, double? direction = null, double? speed = null)
        {
            //do different things if null
            string ans = "";
            ans += "Spawn | %BulletType @" + bulletType + ", ";

            ans += "%Origin ";
            if (origin == null)
            {
                ans += "ScreenCenter, ";
            }
            else
            {
                ans += FormatVector(origin) + ", ";
            }

            ans += "%Direction ";
            if (direction == null)
            {
                ans += "Down, ";
            }
            else
            {
                ans += String.Format("PolarD({0}), ", direction);
            }

            ans += "%Speed ";
            if (speed == null)
            {
                ans += "0";
            }
            else
            {
                ans += speed;
            }
            ans += ";";
            return ans;
        }

        //Spawns bullet of a given type at a given time
        private string SpawnBullet(int indents, double time, string bulletType, Vector2? origin = null, double? direction = null, double? speed = null)
        {
            string ans = "";
            ans += Indentation(indents) + String.Format("At({0})<", time);
            ans += "\r\n";
            ans += Indentation(indents + 1) + SpawnBullet(bulletType, origin, direction, speed);
            ans += "\r\n";
            ans += Indentation(indents) + ">";

            return ans;
        }

        //spawns a ring of bullets going outwards
        private string SpawnRing(int indents, double time, int bulCount, string bulletType, Vector2 origin, double speed, double angleShift)
        {

            string ans = "";
            ans += Indentation(indents) + String.Format("At({0})<", time);
            ans += "\r\n";
            double curAng;
            for (int i = 0; i < bulCount; i++)
            {
                curAng = i * (360 / bulCount);
                if (0 < curAng + angleShift && curAng + angleShift < 180)//gets rid of completely horizontal moving bullets (which are ugly and useless)
                {
                    ans += Indentation(indents + 1) + SpawnBullet(bulletType, origin, curAng + angleShift, speed);
                    ans += "\r\n";
                }

            }
            ans += Indentation(indents) + ">";

            return ans;
        }

        //spawns a series of bullets from the same spawn location going the same way at the same speed within a start and stop time, which looks like a line
        private string SpawnLine(int indents, double start, double end, int bulCount, string bulletType, Vector2 origin, double direction, double speed)
        {
            string ans = "";
            double timeDiff = (end - start) / (bulCount - 1);
            ans += "\r\n";
            for (int i = 0; i < bulCount; i++)
            {
                ans += Indentation(indents) + String.Format("At({0})<", start + timeDiff * i);
                ans += "\r\n";
                ans += Indentation(indents + 1) + SpawnBullet(bulletType, origin, direction, speed);
                ans += "\r\n" + Indentation(indents) + ">";
                ans += "\r\n";
            }
            return ans;
        }

        //spawns a given amount of bullets in a semicircle shape
        private string SpawnWave(int indents, double time, int bulCount, string bulletType, double radius, Vector2 origin, double direction, double speed)
        {
            string ans = "";
            Vector2[] spawnSpots = new Vector2[bulCount];
            double angDiff = 120 / (bulCount - 1);//if you want n bullets, the region must be divided into n-1 smaller regions so after adding each section, you reach the end
            double curAng;
            //figures out where the spawnspots should be around the origin
            for (int i = 0; i < bulCount; i++)
            {
                curAng = direction - 60 + angDiff * i;
                spawnSpots[i] = origin + new Vector2(
                                (int)(radius * Math.Cos(ToRad(curAng))),
                                (int)(radius * Math.Sin(ToRad(curAng))));//needs to be int since it sometimes will go into exponential notation, which dml doesn't recognize
            }
            ans += Indentation(indents) + String.Format("At({0})<", time);
            ans += "\r\n";
            foreach (Vector2 spawnSpot in spawnSpots)
            {
                ans += Indentation(indents + 1) + SpawnBullet(bulletType, spawnSpot, direction, speed);
                ans += "\r\n";
            }
            ans += Indentation(indents) + ">";
            return ans;
        }

        //spawns a series of bullets from the same spawn location going in different directions at the same speed within a start and stop time, which looks like a line sweeping across the screen
        private string SpawnSweep(int indents, double start, double end, int bulCount, string bulletType, Vector2 origin, double minAngle, double maxAngle, int turnDirection, double speed)
        {
            string ans = "";
            double timeDiff = (end - start) / (bulCount - 1);//similar logic to why you need to divide the region into n-1 regions
            double angDiff = (maxAngle - minAngle) / (bulCount - 1);
            double startAngle;
            if (turnDirection == 1)
            {
                startAngle = minAngle;
            }
            else
            {
                startAngle = maxAngle;
            }
            ans += "\r\n";
            for (int i = 0; i < bulCount; i++)
            {
                ans += Indentation(indents) + String.Format("At({0})<", start + timeDiff * i);
                ans += "\r\n";
                ans += Indentation(indents + 1) + SpawnBullet(bulletType, origin, startAngle + turnDirection * angDiff * i, speed);
                ans += "\r\n" + Indentation(indents) + ">";
                ans += "\r\n";
            }
            return ans;
        }

        //creates BurstSpawn in a Generator Bullet's update, which randomly spawns bullets in a range of direction and in a range of speeds at a point in time
        private string SpawnBurst(int indents, double time, int bulCount, string bulletType, double minAngle, double maxAngle, double minSpeed, double maxSpeed)
        {
            string ans = "";

            ans += Indentation(indents) + String.Format("At({0})<", time);
            ans += "\r\n";
            ans += Indentation(indents + 1) + String.Format("BurstSpawn | %BulletType @{0}, %Amount {1}, %SpeedRange Array({2},{3}), %AngleRangeD Array({4},{5});",
                                                                                    bulletType, bulCount, minSpeed, maxSpeed, minAngle, maxAngle);
            ans += "\r\n";
            ans += Indentation(indents) + ">";
            return ans;
        }

        //formats Vector2 into vectors usable in DML
        private string FormatVector(Vector2? v)
        {

            if (v.HasValue)
            {
                return "Vector(" + v.Value.X + "," + v.Value.Y + ")";

            }
            //shouldn't happen
            return "(0,0)";
        }

        //returns correct amount of indentation
        private string Indentation(int indents)
        {
            string ans = "";
            for (int i = 0; i < indents; i++)
            {
                ans += "\t";
            }
            return ans;
        }

        //returns the average energy level of a list of SoundInfo between 2 indices 
        private double AverageEnergyInRange(SoundInfo[] soundInformation, int startIndex, int endIndex)
        {
            double total = 0;
            for (int i = startIndex; i < endIndex; i++)
            {
                total += soundInformation[i].energyLevel;

            }
            return total / (endIndex - startIndex);
        }

        //converts degrees to radians
        private double ToRad(double deg)
        {
            return Math.PI * deg / 180;
        }
    }

    //keeps track of all sound information at a point in time
    class SoundInfo
    {
        public bool isBeat { get; set; }
        public double energyLevel { get; set; }

        public double speed { get; private set; }
        public SoundInfo(bool b, double e)
        {
            isBeat = b;
            energyLevel = e;

            speed = Math.Max(2, Math.Pow(energyLevel * 1.5, .25)); ////decimal power to bring speeds closer together
        }
    }

    //keeps track of a bullet pattern spawn point along with where the pattern can be directed
    class DirectedSpawnLocation
    {
        public Vector2 location { get; private set; }
        public int minAngle { get; private set; }
        public int maxAngle { get; private set; }

        public DirectedSpawnLocation(Vector2 loc, int min, int max)
        {
            location = loc;
            minAngle = min;
            maxAngle = max;
        }
    }
}


