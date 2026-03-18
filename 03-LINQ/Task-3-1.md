# Comparison of generics in different technologies

| Feature | C# (.NET) | Java | C++ (Templates) | TypeScript |
| :--- | :--- | :--- | :--- | :--- |
| **Core Mechanism** | **Reified Generics** | **Type Erasure** | **Code Generation** | **Type Erasure** |
| **Runtime Knowledge** | **Preserved.** The CLR knows the exact type (e.g., `List<int>`). | **Erased.** The JVM only sees `Object` (or bounds). | **Preserved** via entirely separate compiled classes. | **Erased.** The JS engine only sees plain JavaScript variables. |
| **Performance** | High (no boxing/unboxing needed for value types). | Medium (compiler inserts automatic casting; primitives require boxing). | Very High (optimized individually for each type). | N/A (runs as standard JavaScript). |
| **Compiled Code Size** | Small (shares implementation for reference types). | Small (only one implementation exists). | Large (can cause "code bloat" as a new class is made for every type used). | Small (type annotations are completely stripped). |
| **Primitive Types** | Supported natively (e.g., `List<int>`). | Not supported directly (requires wrapper classes like `Integer`). | Supported natively (e.g., `vector<int>`). | N/A (JavaScript only has the `number` type). |

---
```java
import java.util.ArrayList;
import java.util.List;
import java.util.Random;

class RandomizedList<T> {
    private final List<T> items = new ArrayList<>();
    private final Random random = new Random();

    // IsEmpty
    public boolean isEmpty() {
        return items.isEmpty();
    }

    // Add (element)
    public void add(T element) {
        // random.nextInt(2) returns 0 or 1
        if (random.nextInt(2) == 0) {
            items.add(0, element); // add at the beginning
        } else {
            items.add(element);    // add at the end
        }
    }

    // Get (int index)
    public T get(int index) {
        if (isEmpty()) {
            throw new IllegalStateException("Cannot get an element from an empty collection.");
        }
        if (index < 0) {
            throw new IllegalArgumentException("Index cannot be negative.");
        }

        // Max index does not exceed the bounds of the list
        int maxPossibleIndex = Math.min(index, items.size() - 1);

        // Generate a random exact index from 0 to maxPossibleIndex (inclusive)
        int exactIndex = random.nextInt(maxPossibleIndex + 1);

        return items.get(exactIndex);
    }
}

// Test
public class Main {
    public static void main(String[] args) {
        System.out.println("Task 3.1");

        RandomizedList<String> randomList = new RandomizedList<>();
        System.out.println("Is empty at start? " + randomList.isEmpty());

        // Adding elements
        randomList.add("Element A");
        randomList.add("Element B");
        randomList.add("Element C");
        randomList.add("Element D");

        System.out.println("Is empty after adding? " + randomList.isEmpty());

        System.out.println("\nRandomly fetching an element:");
        for (int i = 0; i < 4; i++) {
            System.out.println("- Attempt " + (i + 1) + ": " + randomList.get(2));
        }
    }
}
