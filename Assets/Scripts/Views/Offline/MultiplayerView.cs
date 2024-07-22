using System;
using FishNet;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class MultiplayerView : View
{
	[Header("Main GUI")]
	[SerializeField] private TMP_InputField portField;
	[SerializeField] private Button connectButton;

	public void Start()
	{
		// show the current port
		ushort currentPort = InstanceFinder.NetworkManager.TransportManager.Transport.GetPort();
		string currentClientAddress = InstanceFinder.NetworkManager.TransportManager.Transport.GetClientAddress();
		string fullAddress = $"{currentClientAddress}:{currentPort}";
		portField.text = currentPort.ToString();

		connectButton.onClick.AddListener(() =>
		{
			if (ushort.TryParse(portField.text, out _))
			{
				// start the connection with the old port
				InstanceFinder.ClientManager.StartConnection();
			}
			else
			{
				// start the connection with a new port port
				fullAddress = portField.text;
				currentClientAddress = fullAddress.Split(':')[0];
				currentPort = Convert.ToUInt16(fullAddress.Split(':')[1]);
				InstanceFinder.NetworkManager.TransportManager.Transport.SetPort(currentPort);
				InstanceFinder.NetworkManager.TransportManager.Transport.SetClientAddress(currentClientAddress);
				InstanceFinder.ClientManager.StartConnection();
			}
		});
		
		base.Initialize();
	}
}
