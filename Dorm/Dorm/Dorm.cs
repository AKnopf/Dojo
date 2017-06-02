using System;
using System.Collections.Generic;
using System.Linq;

namespace Dorm
{
    public static class Extensions
    {
        public static IEnumerable<int> UpTo(this int lower, int higher)
        {
            for (int i = lower; i <= higher; i++)
            {
                yield return i;
            }
            yield break;
        }

        public static void Times(this int number, Action action)
        {
            for (int i = 0; i < number-1; i++)
            {
                action();
            }
        }

        public static void Times(this int number, Action<int> action)
        {
            for (int i = 0; i < number - 1; i++)
            {
                action(i);
            }
        }

        public static T Sample<T>(this IList<T> list, Random random)
        {
            return list.SampleWithIndex(random).Item;
        }

        public static (T Item, int Index) SampleWithIndex<T>(this IList<T> list, Random random)
        {
            int i = random.Next(list.Count);
            return (list[i], i);
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }

        public static IEnumerable<R> ForEach<T,R>(this IEnumerable<T> collection, Func<R> action)
        {
            foreach (var item in collection)
            {
                yield return action();
            }
            yield break;
        }

        public static IEnumerable<R> ForEach<T, R>(this IEnumerable<T> collection, Func<T, R> function)
        {
            foreach (var item in collection)
            {
                yield return function(item);
            }
            yield break;
        }
    }

    /// <summary>
    /// This class answers the following question by experiment
    /// 
    /// You have 100 dorm rooms assigned to 100 people. they show up 1 by 1. the first person forgets his assigned room and takes a random room.
    /// Any following person who shows up and has a room that is taken will also take a random room. Otherwise they will take the assigned room.
    /// What is the odds that the last person who shows up ends up in their assigned room?
    /// </summary>
    class Dorm
    {
        public const int NO_OF_ROOMS = 100;
        private const int INDEX_OF_LAST_ROOM = NO_OF_ROOMS - 1;
        private const int INDEX_OF_SECOND_LAST_ROOM = NO_OF_ROOMS - 2;

        public Random Random { get; set; }
        public bool Verbose { get; set; } = false;
        public IList<int> FreeRooms { get; } = new List<int>(INDEX_OF_LAST_ROOM);

        public static void RunExperiment(int n)
        {
            var random = new Random();
            var noSuccesses = 1.UpTo(n).Count(_ => new Dorm { Random = random }.AllocateRooms().LastRoomIsCorrectRoom());
            Console.WriteLine($"Success rate: {Convert.ToDouble(noSuccesses) / n}");
        }

        public Dorm()
        {
            0.UpTo(INDEX_OF_LAST_ROOM).ForEach(FreeRooms.Add);
        }

        public Dorm AllocateRooms()
        {
            TakeRandomFreeRoom(0); // First students takes random room
            1.UpTo(INDEX_OF_SECOND_LAST_ROOM).ForEach(MoveIn);
            return this;
        }

        public bool LastRoomIsCorrectRoom()
        {
            return FreeRooms.Single() == INDEX_OF_LAST_ROOM;
        }

        private void MoveIn(int student)
        {
            if (FreeRooms.Contains(student))
            {
                TakeRoom(student, student);
                if(Verbose)
                    Console.WriteLine($"Student {student} takes assigned room.");
            }
            else
            {
                TakeRandomFreeRoom(student);
            }
        }

        private void TakeRandomFreeRoom(int student)
        {
            int freeRoom = FreeRooms.Sample(Random);
            TakeRoom(student, freeRoom);
            if(Verbose)
            {
                Console.WriteLine($"Student {student} takes random room {freeRoom}.");
                Console.WriteLine($"Free rooms are now: [{string.Join(", ", FreeRooms)}]");
            }
        }

        private void TakeRoom(int student, int room)
        {
            FreeRooms.Remove(room);
        }
    }

}
