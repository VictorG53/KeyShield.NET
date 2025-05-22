public class DonneeDTOResponse(Guid identifiant, byte[] cypher, byte[] iv, byte[] tag)
{
    public Guid Identifiant { get; set; } = identifiant;
    public byte[] Cypher { get; set; } = cypher;
    public byte[] IV { get; set; } = iv;
    public byte[] Tag { get; set; } = tag;
}