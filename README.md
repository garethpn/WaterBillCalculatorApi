

<details>

```
@startuml

skin rose

title Water Meter Diagram \n

start


fork
  :Our Field;
fork again
  :Peter's Upper Field;
fork again
  :Peter's Lower Field;
fork again
  :Car Park;
  note right
    Leak check 1: The readings from...
    * Our Field
    * Peter's 2 Fields
    * Car Park
    .. should all add up to the units
    we have been charged for
  end note
  fork
    :Riverbank Cottage;
  fork again
    :Waunwen Farm House;
  fork again
    :Riverside Barn;
    note right
      Leak check 2: The readings from...
      * Riverbank Cottage
      * Waunwen Farm House
      *Riverside Barn
      .. should all add up to the units
      used by the Car park meter
    end note
  end fork
end fork
stop


@enduml
```

</details>
