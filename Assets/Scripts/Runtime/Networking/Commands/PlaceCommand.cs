using System.IO;
using System.Numerics;
using System.Text;
using UnityEngine;

namespace Networking
{
    // LAYOUT: [0, mushType, long id, vec2int pos, vec2int sourcePos]
    public class PlaceCommand : BaseCommand
    {
        public byte mushType;
        public long id;
        public Vector2Int pos;
        public Vector2Int sourcePos;
        
        public static int COMMAND_LENGTH = 25;

        public PlaceCommand(byte mushType, long id, Vector2Int pos, Vector2Int sourcePos)
        {
            this.mushType = mushType;
            this.id = id;
            this.pos = pos;
            this.sourcePos = sourcePos;
            SerializeCommand();
        }

        public override string ToString()
        {
            return $"{nameof(mushType)}: {mushType}, {nameof(id)}: {id}, {nameof(pos)}: {pos}, {nameof(sourcePos)}: {sourcePos}";
        }

        public PlaceCommand(BinaryReader reader)
            : base(reader)
        { }
        
        public override void SerializeCommand()
        {
            PackageWrapper pw = new PackageWrapper(COMMAND_LENGTH, (byte)CommandType.PLACE);
            pw.Write(mushType);
            pw.Write(id);
            pw.Write(pos);
            pw.Write(sourcePos);
            Buffer = pw.Buffer;
//            Debug.Log("SERIALIZED INTO BUFFER: " + Buffer.ToBitString());
        }

        public override void DeserializeCommand(BinaryReader binaryReader)
        {
            mushType = binaryReader.ReadByte();
            id = binaryReader.ReadBytes(8).DeserializeLong();
            pos = binaryReader.ReadVector2Int();
            sourcePos = binaryReader.ReadVector2Int();
        }
    }
}