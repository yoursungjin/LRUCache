using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LRUCache : MonoBehaviour {

	int capacity;
	int size;
	Dictionary <string,DLinkedListNode> cache;
	DLinkedListNode head;
	DLinkedListNode tail;

	public LRUCache (int capacity){
		this.capacity = capacity;
		cache = new Dictionary<string,DLinkedListNode>(capacity);
		size = 0;
		head = new DLinkedListNode();
		tail = new DLinkedListNode();
		head.prev = null;
		head.next = tail;
		tail.prev = head;
		tail.next = null;
	}

	public GameObject Get(string key) {
		if (cache.ContainsKey(key)) {
			DLinkedListNode node = cache[key];
			MoveNodeToTheFront(node);
			return node.value;
		} else {
			return null;
		}
	}

	public void Set(string key, GameObject value) {
		if (cache.ContainsKey(key)){
			DLinkedListNode node = cache[key];
			//update value
			GameObject gameObject = node.value;
			node.value = value; 
			Destroy(gameObject); //destory the old value
			MoveNodeToTheFront(node);
		} else {
			DLinkedListNode node = new DLinkedListNode();
			node.key = key;
			node.value = value; 
			cache.Add(key,node);
			LinkAtTheFront(node);
			size++;
			if (capacity < size) {
				cache.Remove(tail.prev.key);
				RemoveTheLastNode();
				size--;
			}  
		}
	}


	private void LinkAtTheFront(DLinkedListNode node) {
		//link at the front
		DLinkedListNode headNext = head.next;
		head.next = node;
		node.prev = head;
		node.next = headNext;
		headNext.prev = node;
	}


	private void RemoveTheLastNode() {
		DLinkedListNode tailPrev = tail.prev;
		DLinkedListNode tailPrevPrev = tailPrev.prev;
		tailPrevPrev.next = tail;
		tail.prev= tailPrevPrev;
		Destroy(tailPrev.value); // destroy the gameObject
		tailPrev = null; //destroy the node
	}


	private void MoveNodeToTheFront(DLinkedListNode node) {
		//unlink Node
		DLinkedListNode prevNode = node.prev;
		DLinkedListNode nextNode = node.next;

		prevNode.next = nextNode;
		nextNode.prev = prevNode;

		LinkAtTheFront(node);
	}
}


class DLinkedListNode {
	public string key;  //we need it to delete the cache element from the cache.
	public GameObject value;
	public DLinkedListNode prev;
	public DLinkedListNode next;
}