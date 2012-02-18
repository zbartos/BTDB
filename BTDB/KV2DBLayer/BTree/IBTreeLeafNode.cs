namespace BTDB.KV2DBLayer.BTree
{
    internal interface IBTreeLeafNode
    {
        byte[] GetKey(int idx);
        BTreeLeafMember GetMember(int idx);
    }
}