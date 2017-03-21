using UnityEngine;
using System;
using System.Collections;
using System.IO.Ports;



public class SerialComm : MonoBehaviour {
	
	/* The serial port where the Arduino is connected. */
	[Tooltip("The serial port where the Arduino is connected")]
	public string port = "COM4";
	/* The baudrate of the serial port. */
	[Tooltip("The baudrate of the serial port")]
	public int baudrate = 9600;
	public bool consoleLogging = false;
	
	static private SerialPort stream;


	//private bool portOpen = false;

	void Start()
	{
		Open ();

	}

	void Update()
	{

	}

	void OnApplicationQuit() {
		Debug.Log("Application closing. Trying to close SERIAL.");
		Close ();
	}

	void updateKeys(){

		string tempString = ReadFromArduino ();
		//print (tempString);
		if (tempString != null && tempString.Length >= 8) {

		}
	}

	public void Open () {
		// Opens the serial port
		stream = new SerialPort(port, baudrate);
		stream.ReadTimeout = 50;
		try
		{
			stream.Open();
		}
		catch (System.IO.IOException IOEx)
		{
			if(consoleLogging){
				Debug.Log ("WARNING: CAN NOT OPEN PORT! PORT DOES NOT EXIST!");
			}
		}
		//this.stream.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
	}

	private bool tryReOpen(){

		if(stream.IsOpen){
			return true;
		}

		try
		{
			stream.Open();
		}
		catch (System.IO.IOException IOEx)
		{
			if(consoleLogging){
				Debug.Log ("WARNING: CAN NOT OPEN PORT! PORT DOES NOT EXIST!");
			}
		}

		return stream.IsOpen;

	}
	
	static public void WriteToArduino(string message)
	{
		// Send the request
		if (stream.IsOpen) {
			stream.WriteLine (message);
			stream.BaseStream.Flush ();
		}
	}
	
	public string ReadFromArduino(int timeout = 1)
	{
		/*stream.Write("?");
		stream.BaseStream.Flush();*/

		string temp;
		if (tryReOpen ()) {
			try {
				stream.ReadTimeout = timeout;
				temp = stream.ReadLine ();
				//stream.DiscardInBuffer();
				//stream.BaseStream.Flush();
				if(consoleLogging){
					Debug.Log ("SerialComm: " + temp);
				}
				return temp;
			} catch (TimeoutException ex) { //No data came in time.
				//print (ex);
				return null;
			} catch (System.IO.IOException IoEx){ //The device somehow disconnected
				//print (IoEx);
				Close();
				return null;
			}

		} else {
			return null;
		}

	}

	/*public byte[] ReadBytesFromArduino(int bytesToExpect=0)
	{
		if(byteBuffer.Length < bytesToExpect || byteBuffer[bytesToExpect-1] == null){
			return null;
		}
		return byteBuffer;
		
	}

	public void clearByteBuffer(){
		for(int i=0;i<byteBuffer.Length;i++){
			byteBuffer[i] = null;
		}
		dataIndex = 0;
	}*/
	
	
	public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
	{
		DateTime initialTime = DateTime.Now;
		DateTime nowTime;
		TimeSpan diff = default(TimeSpan);
		
		string dataString = null;
		
		do
		{
			// A single read attempt
			try
			{
				dataString = stream.ReadLine();
			}
			catch (TimeoutException)
			{
				dataString = null;
			}
			
			if (dataString != null)
			{
				callback(dataString);
				yield return null;
			} else
				yield return new WaitForSeconds(0.05f);
			
			nowTime = DateTime.Now;
			diff = nowTime - initialTime;
			
		} while (diff.Milliseconds < timeout);
		
		if (fail != null)
			fail();
		yield return null;
	}
	
	public void Close()
	{
		stream.Close();
	}
}