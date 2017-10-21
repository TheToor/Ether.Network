using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ether.Network
{
    public interface INetSerializable
    {
        void Serialize(BinaryWriter writer);
        void Deserialize(BinaryReader reader);
    }
}
