using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class NetworkPackageManager<T> where T : class
{
    public event System.Action<byte[]> OnRequirePackageTransmit;

    private float mSendSpeed = .2f;
    public float SendSpeed
    {
        get
        {
            if (mSendSpeed < 0.1f)
            {
                return mSendSpeed = 0.1f;
            }
            else
            {
                return mSendSpeed;
            }
        }
        set
        {
            mSendSpeed = value;
        }
    }

    float mNextTick;

    private List<T> mPackages;
    public List<T> Packages
    {
        get
        {
            if (mPackages== null)
            {
                mPackages = new List<T>();
            }
            return mPackages;
        }
    }

    public Queue<T> receivedPackages;
    /// <summary>
    /// Add a new package to the List T to transmit
    /// </summary>
    /// <param name="aPackage"></param>
    public void AddPackage(T aPackage)
    {
        Packages.Add(aPackage);
    }

    public void ReceivedData(byte[] aBytes)
    {
        if(receivedPackages == null)
        {
            receivedPackages = new Queue<T>();
        }

        T[] packages = ReadBytes(aBytes).ToArray();

        for (int i = 0; i < packages.Length; i++)
        {
            receivedPackages.Enqueue(packages[i]);
        }
    }

    public void Tick()
    {
        mNextTick += 1 / SendSpeed * Time.fixedDeltaTime;
        if(mNextTick > 1 && Packages.Count > 0)
        {
            mNextTick = 0;

            if(OnRequirePackageTransmit != null)
            {
                byte[] bytes = CreateBytes();

                Packages.Clear();

                OnRequirePackageTransmit(bytes);
            }
        }
    }

    byte[] CreateBytes()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            formatter.Serialize(ms, this.Packages);
            return ms.ToArray();
        }
    }

    List<T> ReadBytes(byte[] aBytes)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            ms.Write(aBytes, 0, aBytes.Length);
            ms.Seek(0, SeekOrigin.Begin);
            return (List<T>)formatter.Deserialize(ms);
        }
    }
}
