# Calculator App

## Overview

The Calculator App is a web service designed to perform mathematical operations based on inputs received in XML or JSON format. The service processes these inputs, performs the requested operations, and sends back the results in the same format as the request (XML). The application is built using the C# .NET framework and demonstrates key software design principles such as serialization, inheritance, encapsulation, and interface design.

## Features

- **Supports XML:** The service can handle HTTP POST requests with either XML payloads.
- **Dynamic Operation Handling:** The design allows for different XML inputs with minimal or no changes to the C# code.

## Technologies Used

- **Programming Language:** C# .NET Framework
- **Design Patterns:** Serialization, Inheritance, Encapsulation, Interface Design

## Sample Input

### Sample XML Request

```xml
<?xml version="1.0" encoding="UTF-8"?>
<Maths>
    <Operation ID="Plus">
        <Value>2</Value>
        <Value>3</Value>
    </Operation>
    <Operation ID="Multiplication">
        <Value>4</Value>
        <Value>5</Value>
    </Operation>
</Maths>