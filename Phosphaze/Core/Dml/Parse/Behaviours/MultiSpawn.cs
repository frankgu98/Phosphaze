// FIXME: Make this work with relative spawn positions (UGGGGGGGGGGGGGGGGH)

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Phosphaze.Core.Dml.Parse.Behaviours
{
    // Man this was annoying to make. The one time you want duck typing.

	public class MultiSpawn : Behaviour
	{

		/*
		 * MultiSpawn | %BulletType,
		 * 				[%Origin | %Origins],
		 * 			  	[%Direction | %Directions],
		 * 			  	[%Speed | %Speeds]
		 */

		public MultiSpawn() { }

		private enum ParamState { None, Single, Multi }

		ParamState originParam = ParamState.None;
		ParamState directionParam = ParamState.None;
		ParamState speedParam = ParamState.None;
		bool multi = false;

		int paramCount = 0;

		private ParamState GetSingleOrPlural(string[] parameters, string single, string plural)
		{
			ParamState state = ParamState.None;

			if (parameters.Contains(single))
				state = ParamState.Single;

			if (parameters.Contains(plural))
			{
				if (state == ParamState.Single)
					throw new BehaviourException(
						String.Format("MultiSpawn cannot have both {0} and {1} defined.", single, plural)
						);
				state = ParamState.Multi;
			}
			return state;
		}

		public void Initialize(string[] parameters)
		{

			originParam = GetSingleOrPlural(parameters, "Origin", "Origins");
			directionParam = GetSingleOrPlural(parameters, "Direction", "Directions");
			speedParam = GetSingleOrPlural(parameters, "Speed", "Speeds");

			if (originParam == ParamState.Multi || 
				directionParam == ParamState.Multi || 
				speedParam == ParamState.Multi)
				multi = true;

			foreach (string param in parameters)
				if (param == "Param")
					paramCount++;
		}

		public void Perform(
            CodeBlock block,
            Stack<DmlObject> stack,
            Dictionary<string, DmlObject> locals,
            DmlObject bullet, DmlSystem system)
        {
        	var paramNames = new string[paramCount];
            var values = new DmlObject[paramCount];

            DmlObject top;
            List<DmlObject> param;

            for (int i = 0; i < paramCount; i++)
            {
                top = stack.Pop();
                param = (List<DmlObject>)top.Value;
                paramNames[i] = (string)(param[0].Value);
                values[i] = param[1];
            }

            DmlObject originObj = null, directionObj = null, speedObj = null;
            if (speedParam != ParamState.None)
            	speedObj = stack.Pop();
            if (directionParam != ParamState.None)
            	directionObj = stack.Pop();
            if (originParam != ParamState.None)
            	originObj = stack.Pop();

            var factory = (DmlBulletFactory)(stack.Pop().Value);

            if (multi)
            {
                Vector2? origin = null, direction = null;
                double speed = 0;
                List<DmlObject> origins = null, directions = null, speeds = null;

            	if (originParam == ParamState.None)
            		origin = ((DmlBullet)bullet.Value).Position;
            	else if (originParam == ParamState.Single)
            		origin = (Vector2)(originObj.Value);
            	else
            		origins = (List<DmlObject>)(originObj.Value);

            	if (directionParam == ParamState.None)
            		direction = new Vector2(0, 1);
            	else if (directionParam == ParamState.Single)
            		direction = (Vector2)(directionObj.Value);
            	else
            		directions = (List<DmlObject>)(directionObj.Value);

            	if (speedParam == ParamState.None)
            		speed = 0;
            	else if (speedParam == ParamState.Single)
            		speed = (double)(speedObj.Value);
            	else
            		speeds = (List<DmlObject>)(speedObj.Value);

            	DmlBullet parent = null;
            	if (bullet != null)
            		parent = (DmlBullet)(bullet.Value);

            	if (origins == null)
            	{
            		if (directions == null)
            		{
            			if (speeds == null)
            				Spawn(origin.Value, direction.Value, speed, paramNames, values, parent, factory, system);
            			else
            				Spawn(origin.Value, direction.Value, speeds, paramNames, values, parent, factory, system);
            		}
            		else
            		{
            			if (speeds == null)
            				Spawn(origin.Value, directions, speed, paramNames, values, parent, factory, system);
            			else
            				Spawn(origin.Value, directions, speeds, paramNames, values, parent, factory, system);
            		}
            	}
            	else 
            	{
            		if (directions == null)
            		{
                        if (speeds == null)
                            Spawn(origins, direction.Value, speed, paramNames, values, parent, factory, system);
                        else
                            Spawn(origins, direction.Value, speeds, paramNames, values, parent, factory, system);
            		}
            		else
            		{
            			if (speeds == null)
            				Spawn(origins, directions, speed, paramNames, values, parent, factory, system);
            			else
            				Spawn(origins, directions, speeds, paramNames, values, parent, factory, system);
            		}	
            	}
            }
            else 
            {
                Vector2 origin;
            	if (originParam == ParamState.None)
            		origin = ((DmlBullet)bullet.Value).Origin;
            	else
            		origin = (Vector2)(originObj.Value);

            	DmlObject newObj = factory.Instantiate(origin, system);
            	DmlBullet newBullet = (DmlBullet)newObj.Value;

            	if (directionParam != ParamState.None)
            	    newBullet.Direction = (Vector2)(directionObj.Value);

            	if (speedParam != ParamState.None)
            	    newBullet.Speed = (double)(speedObj.Value);

            	for (int i = 0; i < paramCount; i++)
            	    newObj.SetVar(paramNames[i], values[i]);

            	if (bullet != null)
            	    // We have to check if bullet is null because Spawn is one of the few behaviours
            	    // that can be used in a Timeline. When the Timeline's code is executed, `null`
            	    // is sent in for `bullet`.
            	    ((DmlBullet)bullet.Value).Children.Add(newBullet);	
            }
        }

        private void Spawn(
        	Vector2 origin, Vector2 direction, double speed,
        	string[] paramNames, DmlObject[] values,
        	DmlBullet parent, DmlBulletFactory factory, DmlSystem system) 
        {
        	DmlObject newObj = factory.Instantiate(origin, system);
        	DmlBullet newBullet = (DmlBullet)newObj.Value;
            newBullet.Direction = direction;
            newBullet.Speed = speed;

            for (int i = 0; i < paramCount; i++)
                newObj.SetVar(paramNames[i], values[i]);

            if (parent != null)
                parent.Children.Add(newBullet);
            system.AddBullet(newBullet);
        }

        private void Spawn(
        	List<DmlObject> origins, Vector2 direction, double speed, 
        	string[] paramNames, DmlObject[] values,
        	DmlBullet parent, DmlBulletFactory factory, DmlSystem system) 
        {
        	bool local = parent != null;

        	Vector2 origin;
        	DmlObject newObj;
        	DmlBullet newBullet;

        	foreach (DmlObject o in origins)
        	{
        		origin = (Vector2)(o.Value);

        		newObj = factory.Instantiate(origin, system);
        		newBullet = (DmlBullet)(newObj.Value);

        		newBullet.Direction = direction;
        		newBullet.Speed = speed;

        		for (int i = 0; i < paramCount; i++)
        		    newObj.SetVar(paramNames[i], values[i]);

        		if (local)
        		    parent.Children.Add(newBullet);
        		system.AddBullet(newBullet);
        	}
        }

        private void Spawn(
        	Vector2 origin, List<DmlObject> directions, double speed, 
        	string[] paramNames, DmlObject[] values,
        	DmlBullet parent, DmlBulletFactory factory, DmlSystem system) 
        {
        	bool local = parent != null;

        	Vector2 direction;
        	DmlObject newObj;
        	DmlBullet newBullet;

        	foreach (DmlObject d in directions)
        	{
        		direction = (Vector2)(d.Value);

        		newObj = factory.Instantiate(origin, system);
        		newBullet = (DmlBullet)(newObj.Value);

        		newBullet.Direction = direction;
        		newBullet.Speed = speed;

        		for (int i = 0; i < paramCount; i++)
        		    newObj.SetVar(paramNames[i], values[i]);

        		if (local)
        		    parent.Children.Add(newBullet);
        		system.AddBullet(newBullet);
        	}
        }

        private void Spawn(
        	Vector2 origin, Vector2 direction, List<DmlObject> speeds, 
        	string[] paramNames, DmlObject[] values,
        	DmlBullet parent, DmlBulletFactory factory, DmlSystem system) 
        {
        	bool local = parent != null;

        	double speed;
        	DmlObject newObj;
        	DmlBullet newBullet;

        	foreach (DmlObject s in speeds)
        	{
        		speed = (double)(s.Value);

        		newObj = factory.Instantiate(origin, system);
        		newBullet = (DmlBullet)(newObj.Value);

        		newBullet.Direction = direction;
        		newBullet.Speed = speed;

        		for (int i = 0; i < paramCount; i++)
        		    newObj.SetVar(paramNames[i], values[i]);

        		if (local)
        		    parent.Children.Add(newBullet);
        		system.AddBullet(newBullet);
        	}
        }

        private void Spawn(
        	List<DmlObject> origins, List<DmlObject> directions, double speed, 
        	string[] paramNames, DmlObject[] values,
        	DmlBullet parent, DmlBulletFactory factory, DmlSystem system) 
        {
        	if (origins.Count != directions.Count)
        		throw new BehaviourException("Origins and Directions must have the same number of elements.");

        	bool local = parent != null;

        	Vector2 origin, direction;
        	DmlObject newObj;
        	DmlBullet newBullet;

        	for (int i = 0; i < origins.Count; i++)
        	{
        		origin = (Vector2)(origins[i].Value);
        		direction = (Vector2)(directions[i].Value);

        		newObj = factory.Instantiate(origin, system);
        		newBullet = (DmlBullet)(newObj.Value);

        		newBullet.Direction = direction;
        		newBullet.Speed = speed;

        		for (int j = 0; j < paramCount; j++)
        		    newObj.SetVar(paramNames[j], values[j]);

        		if (local)
        		    parent.Children.Add(newBullet);
        		system.AddBullet(newBullet);
        	}
        }

        private void Spawn(
        	List<DmlObject> origins, Vector2 direction, List<DmlObject> speeds, 
        	string[] paramNames, DmlObject[] values,
        	DmlBullet parent, DmlBulletFactory factory, DmlSystem system) 
        {
        	if (origins.Count != speeds.Count)
        		throw new BehaviourException("Origins and Speeds must have the same number of elements.");

        	bool local = parent != null;

        	Vector2 origin;
        	double speed;
        	DmlObject newObj;
        	DmlBullet newBullet;

        	for (int i = 0; i < origins.Count; i++)
        	{
        		origin = (Vector2)(origins[i].Value);
        		speed = (double)(speeds[i].Value);

        		newObj = factory.Instantiate(origin, system);
        		newBullet = (DmlBullet)(newObj.Value);

        		newBullet.Direction = direction;
        		newBullet.Speed = speed;

        		for (int j = 0; j < paramCount; j++)
        		    newObj.SetVar(paramNames[j], values[j]);

        		if (local)
        		    parent.Children.Add(newBullet);
        		system.AddBullet(newBullet);
        	}
        }

        private void Spawn(
        	Vector2 origin, List<DmlObject> directions, List<DmlObject> speeds, 
        	string[] paramNames, DmlObject[] values,
        	DmlBullet parent, DmlBulletFactory factory, DmlSystem system) 
        {
        	if (directions.Count != speeds.Count)
        		throw new BehaviourException("Directions and Speeds must have the same number of elements.");

        	bool local = parent != null;

        	Vector2 direction;
        	double speed;
        	DmlObject newObj;
        	DmlBullet newBullet;

        	for (int i = 0; i < directions.Count; i++)
        	{
        		direction = (Vector2)(directions[i].Value);
        		speed = (double)(speeds[i].Value);

        		newObj = factory.Instantiate(origin, system);
        		newBullet = (DmlBullet)(newObj.Value);

        		newBullet.Direction = direction;
        		newBullet.Speed = speed;

        		for (int j = 0; j < paramCount; j++)
        		    newObj.SetVar(paramNames[j], values[j]);

        		if (local)
        		    parent.Children.Add(newBullet);
        		system.AddBullet(newBullet);
        	}
        }

        private void Spawn(
        	List<DmlObject> origins, List<DmlObject> directions, List<DmlObject> speeds, 
        	string[] paramNames, DmlObject[] values,
        	DmlBullet parent, DmlBulletFactory factory, DmlSystem system) 
        {
        	if (origins.Count != directions.Count || origins.Count != speeds.Count || directions.Count != speeds.Count)
        		throw new BehaviourException("Origins, Directions and Speeds must all have the same number of elements.");

        	bool local = parent != null;

        	Vector2 origin, direction;
        	double speed;
        	DmlObject newObj;
        	DmlBullet newBullet;

        	for (int i = 0; i < origins.Count; i++)
        	{
        		origin = (Vector2)(origins[i].Value);
        		direction = (Vector2)(directions[i].Value);
        		speed = (double)(speeds[i].Value);

        		newObj = factory.Instantiate(origin, system);
        		newBullet = (DmlBullet)(newObj.Value);

        		newBullet.Direction = direction;
        		newBullet.Speed = speed;

        		for (int j = 0; j < paramCount; j++)
        		    newObj.SetVar(paramNames[j], values[j]);

        		if (local)
        		    parent.Children.Add(newBullet);
        		system.AddBullet(newBullet);
        	}
        }

	}

}