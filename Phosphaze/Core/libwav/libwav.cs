//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.5
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


public class libwav {
  public static SWIGTYPE_p_unsigned_char WAVE_MEDIASUBTYPE_PCM {
    get {
      global::System.IntPtr cPtr = libwavPINVOKE.WAVE_MEDIASUBTYPE_PCM_get();
      SWIGTYPE_p_unsigned_char ret = (cPtr == global::System.IntPtr.Zero) ? null : new SWIGTYPE_p_unsigned_char(cPtr, false);
      if (libwavPINVOKE.SWIGPendingException.Pending) throw libwavPINVOKE.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public static readonly double M_PI = libwavPINVOKE.M_PI_get();
}