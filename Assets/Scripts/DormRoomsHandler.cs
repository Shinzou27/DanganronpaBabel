using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DormRoomsHandler : MonoBehaviour
{
    [SerializeField] private List<CharacterSO> characters;
    [SerializeField] private List<GameObject> rooms;
    // Start is called before the first frame update
    void Start()
    {
        ShuffleRooms();
        Singleton.Instance.OnDormRoomExited += ShuffleRooms;
        Singleton.Instance.OnDormRoomReplace += ReplaceRooms;
    }

    private void ReplaceRooms(object sender, Singleton.ReplaceRoomEventArgs e)
    {
        DormRoom current = null;
        DormRoom roomToReplace = null;
        foreach (GameObject roomGameObject in rooms) {
            DormRoom room = roomGameObject.GetComponent<DormRoom>();
            if (room.GetCharacter() == e.current) {
                current = room;
            } else if (room.GetCharacter() == e.toReplace) {
                roomToReplace = room;
            }
        }
        if (current != null && roomToReplace != null) {
            SetGuest(current.gameObject, e.toReplace);
            SetGuest(roomToReplace.gameObject, e.current);
            GameFlow.Instance.AddToVisit(e.toReplace);
        }

    }

    private void ShuffleRooms(object sender, Singleton.DormRoomEventArgs e)
    {
        List<int> currentAssignment = new();
        List<int> visited = new();
        foreach (GameObject room in rooms) {
            currentAssignment.Add(room.GetComponent<DormRoom>().GetCharacter().characterIndex);
        }
        int visitedIndex = 0;
        foreach (bool visitedChar in GameFlow.Instance.GetVisitedCharacters()) {
            if (visitedChar) {
                visited.Add(visitedIndex);
                visitedIndex++;
            }
        }
        List<int> indexes = RoomIndexGenerator.GenerateRandomIntList(currentAssignment, visited);
        
        DormRoom enteredDormRoom = e.dormRoom;
        CharacterSO characterOnEnteredDormRoom = e.dormRoom.GetCharacter();
        // Debug.Log(characterOnEnteredDormRoom.name + " está no " + enteredDormRoom.gameObject.name);
        CharacterSO characterAssignedToEnteredDormRoom = null;
        GameObject dormRoomHavingCharacterOfEnteredDormRoom = null;
        
        foreach (CharacterSO character in characters) {
            int index = indexes[character.characterIndex];
            SetGuest(rooms[index], character);
            if (rooms[index] == enteredDormRoom.gameObject) {
                characterAssignedToEnteredDormRoom = character;
            }
        }
        dormRoomHavingCharacterOfEnteredDormRoom = rooms.Find(
            (room) => room.GetComponent<DormRoom>().GetCharacter() == characterOnEnteredDormRoom
        );
        SetGuest(enteredDormRoom.gameObject, characterOnEnteredDormRoom);
        SetGuest(dormRoomHavingCharacterOfEnteredDormRoom, characterAssignedToEnteredDormRoom);
        PrintDormRoomState();
    }

    private void ShuffleRooms() {
        List<int> indexes = RoomIndexGenerator.GenerateRandomIntList(rooms.Count);
        foreach (CharacterSO character in characters) {
            int index = indexes[character.characterIndex];
            SetGuest(rooms[index], character);
        }
        PrintDormRoomState();
    }
    private void PrintDormRoomState() {
        int i = 1;
        StringBuilder toPrint = new();
        foreach (GameObject room in rooms) {
            if(GameFlow.Instance.characterVisitOrder.Contains(room.GetComponent<DormRoom>().GetCharacter())) {
                string name = room.GetComponent<DormRoom>().GetCharacter().characterName;
                toPrint.Append(name + " está no quarto " + i + '\n');
            }
            i++;
        }
        toPrint.Append("------------------------");
        Debug.Log(toPrint);
    }
    private void SetGuest(GameObject room, CharacterSO character) {
        room.GetComponent<DormRoomVisual>().SetCharacter(character);
        room.GetComponent<DormRoom>().SetCharacter(character);
        room.GetComponent<DormRoom>().SpawnBuffObject();
    }
}
