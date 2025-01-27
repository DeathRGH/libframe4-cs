namespace libframe4
{
    public class KernelMemoryEntry
    {
        public string name;
        public ulong start;
        public ulong end;
        public ulong offset;
        public uint prot;
    }

    public class KernelVmMap
    {
        public KernelMemoryEntry[] entries;

        /// <summary>
        /// Initializes KernelVmMap class with memory entries
        /// </summary>
        /// <param name="entries">Process memory entries</param>
        /// <returns></returns>
        public KernelVmMap(KernelMemoryEntry[] entries)
        {
            this.entries = entries;
        }

        /// <summary>
        /// Finds a virtual memory entry based off name
        /// </summary>
        /// <param name="name">Virtual memory entry name</param>
        /// <param name="contains">Condition to check if entry name contains name</param>
        /// <returns></returns>
        public KernelMemoryEntry FindEntry(string name, bool contains = false)
        {
            foreach (KernelMemoryEntry entry in entries)
            {
                if (contains)
                {
                    if (entry.name.Contains(name))
                    {
                        return entry;
                    }
                }
                else
                {
                    if (entry.name == name)
                    {
                        return entry;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Finds a virtual memory entry based off name
        /// </summary>
        /// <param name="name">Virtual memory entry name</param>
        /// <param name="prot">Virtual memory entry protection level</param>
        /// <returns></returns>
        public KernelMemoryEntry FindEntry(string name, int prot)
        {
            foreach (KernelMemoryEntry entry in entries)
            {
                if (entry.name == name && entry.prot == prot)
                {
                    return entry;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds a virtual memory entry based off size
        /// </summary>
        /// <param name="size">Virtual memory entry size</param>
        /// <returns></returns>
        public KernelMemoryEntry FindEntry(ulong size)
        {
            foreach (KernelMemoryEntry entry in entries)
            {
                if ((entry.start - entry.end) == size)
                {
                    return entry;
                }
            }

            return null;
        }
    }
}
