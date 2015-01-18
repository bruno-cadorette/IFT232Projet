using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Configuration
{
    [DataContract]
    public class GameConfigData
    {
        [DataMember]
        public IEnumerable<BuildableEntity> Entities { get; set; }
        [DataMember]
        public LandscapeConfig Landscape { get; set; }

        public GameConfigData()
        {
            
        }
        public static GameConfigData Load(string fileName)
        {
            using (var fileStream = File.Open(fileName, FileMode.Open))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(GameConfigData));
                fileStream.Position = 0;
                return serializer.ReadObject(fileStream) as GameConfigData;
            }
        }

        public void Save(string fileName)
        {
            using (var fileStream = File.Create(fileName))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(GameConfigData));
                serializer.WriteObject(fileStream, this);
            }
        }
    }
}
