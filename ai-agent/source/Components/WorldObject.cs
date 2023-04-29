using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AIAgent.Types;
using UnityEngine;

namespace AIAgent.Components
{
    public class WorldObject : MonoBehaviour
    {
        #region Variables
        
        private AgentWorldObjectMemory _object;

        [Header("Location")]
        public string zone;
        public string room;
        public string objectName;

        #endregion

        #region Lifecycle

        private void Awake()
        {
            _object = new AgentWorldObjectMemory{ Zone = zone, Room = room, ObjectName = objectName};
            _object.Id = GenerateID();
            gameObject.name = name;
        }

        #endregion

        #region Methods

        private int GenerateID()
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(_object.Zone + _object.Room + _object.ObjectName);
            var hash = md5.ComputeHash(inputBytes);
            return BitConverter.ToInt32(hash, 0);
        }
        
        public static WorldObject FindObject(string zone, string room, string name)
        {
            var worldObjects = FindObjectsOfType<WorldObject>();
            return worldObjects.FirstOrDefault(worldObject => worldObject._object.Zone == zone && worldObject._object.Room == room && worldObject._object.ObjectName == name);
        }
        
        public static WorldObject FindObject(AgentWorldObjectMemory value)
        {
            return FindObject(value.Zone, value.Room, value.ObjectName);
        }

        public static List<AgentWorldObjectMemory> FindAllObjectsInWorld()
        {
            return FindObjectsOfType<WorldObject>().Select(it => it.Object).ToList();
        }

        #endregion

        #region Properties

        public int ID => _object.Id;
        public string Zone => _object.Zone;
        public string Room => _object.Room;
        public string Name => _object.ObjectName;
        public AgentWorldObjectMemory Object => _object; 

        #endregion
    }
}