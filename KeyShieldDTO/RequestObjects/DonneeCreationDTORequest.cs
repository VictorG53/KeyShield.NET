namespace KeyShieldDTO.RequestObjects;

public class DonneeCreationDTORequest(byte[] cypher, byte[] iv, byte[] tag)
{
    public byte[] Cypher { get; set; } = cypher;
    public byte[] IV { get; set; } = iv;
    public byte[] Tag { get; set; } = tag;
}