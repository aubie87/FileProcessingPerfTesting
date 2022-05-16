## FileProcessingPerfTesting
Test and optimize multi-file processing and storage.

Starting with simple, sequential processing and progressing to using
async/await, dedicated threads and producer/consumer queues - determine
ideal processing configuration around runtime and memory usage.

Likely heavy dependence on size of input files.

All database "saves" are done via a "unit of work". The DbContext
goes out of scope at the conclusion of the save operation. 

Once saved, loaded objects should also go "out of scope". Therefore, memory
requirements should remain very tame no mater the size of the input files.

#### Summary of Testing Scenarios
1. Sequencial file loading and bulk saving.
   - What is the ideal bulk size to save? DB dependent?
2. Sequencial with async file loading and async bulk saving.
   - Should be somewhat faster than #1 
3. Parallel.ForEachAsync file loading and async bulk saving.
   - Parallelize input file processing but otherwise the same as #2.
   - Concern saving to DB from multiple threads.
4. Same as #3 but save objects to a queue and bulk save in 1 dedicated thread.
   - Producer/Consumer queue with many producers and 1 consumer
   - Very possible queue could be overwhelmed with producer data.

