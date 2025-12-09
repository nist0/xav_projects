# KiCad Eeschema Reference (AI-Oriented)

## 1. Overview

Eeschema is the schematic capture tool of KiCad.  
It supports hierarchical designs, symbol libraries, electrical rules checking, net management, footprint assignment, cross-probing with PCB Editor, and integration with SPICE simulation.

---

## 2. Application Structure

### 2.1 Major Components

- Schematic canvas  
- Symbol libraries  
- Hierarchical sheets  
- Electrical connectivity engine  
- ERC engine  
- Schematic setup system  
- Footprint association system  
- Cross-probing subsystem  
- SPICE subsystem  

### 2.2 File Types

- `.kicad_sch` : schematic  
- `.kicad_sym` : symbol library  
- `.kicad_pro` : project settings  
- `.kicad_prl` : project local configuration  
- `.kicad_dbl` : design block library  

---

## 3. User Interface Model

### 3.1 Toolbars

- Top toolbar: file operations, undo/redo, zoom.  
- Left toolbar: display options, grids, hierarchical navigation.  
- Right toolbar: placement tools (symbols, wires, labels, buses, graphics).

### 3.2 Panels

- Properties Manager  
- Hierarchy Navigator  
- Search Panel  

### 3.3 Canvas Interaction

- Panning  
- Zooming  
- Grid snapping  
- Object snapping  

---

## 4. Grids and Snapping

### 4.1 Grid Rules

- Default grid: 50 mil (1.27 mm).  
- Grid controls placement of wires, labels, and symbols.  
- Misaligned elements can be corrected via “Align Elements to Grid”.

### 4.2 Snapping Types

- Grid snapping  
- Connected-object snapping  

### 4.3 Snapping Overrides

- Disable grid snapping: `Ctrl`  
- Disable object snapping: `Shift`  

---

## 5. Selection Rules

### 5.1 Methods

- Single-click selection  
- Selection box:
  - Left-to-right: exclusive  
  - Right-to-left: inclusive  

### 5.2 Modifiers

- Add to selection: Ctrl/Cmd  
- Remove from selection: Ctrl+Shift / Cmd+Shift  

### 5.3 Selection Filter

Allows enabling/disabling selection by object type.

---

## 6. Symbols

### 6.1 Symbol Entities

A symbol contains:

- Graphical body  
- Pins  
- Fields  
- Electrical attributes  
- Footprint filters  
- Aliases  
- SPICE metadata  

### 6.2 Placement Rules

- Symbols snap to grid.  
- Rotation allowed.  
- Mirroring allowed.  
- Multi-unit symbols supported.  

### 6.3 Modification

- Instance-level editing  
- Library-level editing  
- Update symbol from library (manual refresh)

---

## 7. Pins

### 7.1 Pin Properties

- Name  
- Number  
- Electrical type
  - Input  
  - Output  
  - Bidirectional  
  - Passive  
  - Power Input  
  - Power Output  
  - Open Drain / Open Collector  
  - Tri-state  
  - Not Connected  

### 7.2 Alternate Pin Functions

Pins may define alternate functions which can be selected per instance.

---

## 8. Symbol Libraries

### 8.1 Library Types

- Global libraries (sym-lib-table)  
- Project libraries  
- External libraries  

### 8.2 Library Table Fields

- Nickname  
- Library path  
- Plugin type  
- Options  

### 8.3 Operations

- Add library  
- Remove library  
- Reorder libraries  
- Create symbol  
- Duplicate symbol  
- Edit symbol  

---

## 9. Electrical Connectivity

### 9.1 Connectivity Rules

- A connection is defined when a wire endpoint, label, or pin occupies exactly the same coordinate.  
- Visual overlap without coordinate match does not create a net.  

### 9.2 Wires

- Orthogonal/angled based on settings  
- Connect only at endpoints  

### 9.3 Junctions

- Explicit connection indicator  
- Auto-inserted when necessary  

### 9.4 No-Connect Flag

Indicates that a pin is intentionally unconnected.

---

## 10. Labels and Nets

### 10.1 Label Types

- Local label  
- Global label  
- Hierarchical label  

### 10.2 Net Naming

Nets may be:

- explicitly named (via labels)  
- implicitly generated (e.g., Net-(R1-Pad1))  

### 10.3 Global Signals

Power symbols define global nets.

---

## 11. Hierarchical Schematics

### 11.1 Components

- Parent sheet  
- Child sheets  
- Hierarchical pins  
- Hierarchical labels  

### 11.2 Sheet Rules

- Each sheet maps hierarchical labels to hierarchical pins.  
- Complex hierarchy allows multiple instances of the same sheet.

---

## 12. ERC (Electrical Rules Check)

### 12.1 Error Categories

- Hard errors  
- Warnings  
- Informational messages  

### 12.2 Checked Conditions

- Conflicting pin types  
- Power input pins lacking sources  
- Floating pins  
- Missing connections  
- Incorrect hierarchical connections  

### 12.3 Configuration

ERC rules are configurable per project.

---

## 13. Footprint Assignment

### 13.1 Methods

- Per-symbol assignment  
- Via Symbol Chooser  
- Via “Assign Footprints” tool (recommended for batch operations)

### 13.2 Footprint Filters

Match footprints based on wildcard patterns.

---

## 14. Forward and Back Annotation

### 14.1 Forward Annotation

Schematic → PCB:

- Nets  
- Footprints  
- Net classes  
- Reference updates  

### 14.2 Back Annotation

PCB → Schematic:

- Reference updates  
- Changes to footprints or removed parts  
(Generally less used.)

---

## 15. Design Blocks

### 15.1 Definition

Reusable schematic fragments containing symbols, nets, labels, and graphics.

### 15.2 Operations

- Save selection as design block  
- Insert design block  
- Manage design block libraries  

---

## 16. SPICE Simulation Integration

### 16.1 Model Assignment

Symbols may include:

- prefix  
- device model  
- subcircuit association  
- pin mapping  

### 16.2 Directives

Supported directives include:

- `.model`  
- `.include`  
- `.lib`  
- `.tran`  
- `.ac`  
- `.dc`  
- `.options`  
- `.ic`  

### 16.3 Simulation Types

- DC  
- AC  
- Transient  
- Parametric sweeps  
- Noise analysis  

---

## 17. Search, Inspection, Cross-Probing

### 17.1 Search Tools

Support searching:

- symbols  
- references  
- values  
- nets  
- labels  
- text  

### 17.2 Net Highlighting

Visually highlights complete electrical nets.

### 17.3 Cross-Probing

Synchronizes selection between schematic and PCB.

---

## 18. Miscellaneous Features

### 18.1 Graphics

- Shapes  
- Text  
- Images  
- Tables  

### 18.2 Batch Editing

- Multi-selection  
- Properties Manager  
- Field overrides  

### 18.3 Schematic Setup

Configuration for:

- ERC  
- Net classes  
- Title block  
- Page formatting  

---

## 19. Action Reference (Core Operations)

### 19.1 Placement

- Place symbol  
- Place wire  
- Place junction  
- Place label  
- Place hierarchical sheet  
- Place sheet pin  

### 19.2 Editing

- Move  
- Drag  
- Rotate  
- Mirror  
- Delete  
- Edit properties  

### 19.3 Navigation

- Zoom in/out  
- Zoom to fit  
- Go to parent sheet  
- Navigate hierarchy  

---

## 20. Summary

This document defines the structural, behavioral, and operational rules of KiCad Eeschema in a format suitable for machine ingestion.  
It provides normalized terminology and hierarchical organization for AI reference usage.
