using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TrashCan : MonoBehaviour
{
    [Header("UI設定")]
    [SerializeField] private TextMeshProUGUI counterText; // カウンター表示用
    
    [Header("ゲーム設定")]
    [SerializeField] private int totalLeaves = 6; // 全体の葉っぱの数
    
    private int collectedLeaves = 0; // 集めた葉っぱの数
    private HashSet<Leaf> leavesInTrash = new HashSet<Leaf>(); // ゴミ箱内の葉っぱを追跡
    
    void Start()
    {
        UpdateCounterText();
        
        // Colliderの確認
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogError("TrashCanにColliderが必要です！");
        }
        else if (!col.isTrigger)
        {
            Debug.LogError("TrashCanのColliderのIs Triggerをオンにしてください！");
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        Leaf leaf = other.GetComponent<Leaf>();
        if (leaf != null)
        {
            leavesInTrash.Add(leaf);
            Debug.Log($"{other.gameObject.name} がゴミ箱に入りました");
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        // ゴミ箱内にある葉っぱで、ドラッグが終わった（マウスが離された）ものを削除
        Leaf leaf = other.GetComponent<Leaf>();
        if (leaf != null && leavesInTrash.Contains(leaf))
        {
            if (leaf.IsReleased())
            {
                CollectLeaf(leaf);
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        Leaf leaf = other.GetComponent<Leaf>();
        if (leaf != null)
        {
            leavesInTrash.Remove(leaf);
            Debug.Log($"{other.gameObject.name} がゴミ箱から出ました");
        }
    }
    
    void CollectLeaf(Leaf leaf)
    {
        // カウントを増やす
        collectedLeaves++;
        UpdateCounterText();
        
        Debug.Log($"葉っぱを回収しました！ ({collectedLeaves}/{totalLeaves})");
        
        // 葉っぱを削除
        leavesInTrash.Remove(leaf);
        leaf.Dispose();
        
        // すべての葉っぱを集めた時
        if (collectedLeaves >= totalLeaves)
        {
            OnTaskComplete();
        }
    }
    
    void UpdateCounterText()
    {
        if (counterText != null)
        {
            counterText.text = $"LeafCount: {collectedLeaves}/{totalLeaves}";
        }
    }
    
    void OnTaskComplete()
    {
        Debug.Log("タスク完了!");
        // ここにタスク完了時の処理を追加できます
        if (counterText != null)
        {
            counterText.text = "Clear!";
        }
    }
}
