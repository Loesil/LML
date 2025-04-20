# Media Library Manager

A modern C# application for managing your media library, built with WPF and .NET 8.0. This application helps you organize and manage your media files (music, videos, and images) with features like metadata extraction, file organization, and playlist management.

## Features

- Modern, dark-themed UI with WPF
- Support for various media formats (audio, video, images)
- Automatic metadata extraction using TagLib#
- File organization with customizable patterns
- Duplicate file detection
- Playlist management
- Tag-based organization
- Search and filter capabilities

## Project Structure

The solution follows a clean architecture approach with these main projects:

- **LML.Core**: Contains domain models, interfaces, and business logic
- **LML.Infrastructure**: Implements core interfaces and provides services
- **LML.WPF**: WPF-based user interface

## Requirements

- .NET 8.0 SDK
- Windows OS (for WPF support)

## Getting Started

1. Clone the repository
2. Open the solution in Visual Studio 2022 or later
3. Build and run the solution

## Usage

### Adding Media Files

- Click "Add Files" to select individual media files
- Click "Add Folder" to import an entire directory of media files

### Organizing Files

The application can organize your files using patterns like:

```
{Type}/{Artist}/{Album}/{Title}
```

Available pattern variables:

- {Type}: Media type (Audio, Video, Image)
- {Artist}: Artist name
- {Album}: Album name
- {Title}: File title

### Filtering and Searching

- Use the search box to find media files
- Filter by type using the navigation menu
- Create custom filters using tags

## Development

### Adding New Features

1. Define interfaces in the Core project
2. Implement features in the Infrastructure project
3. Add UI components in the WPF project

### Building

```bash
dotnet build
```

### Running

```bash
dotnet run --project LML.WPF
```

## Dependencies

- TagLib# (2.3.0): Media metadata extraction
- System.Text.Json: JSON serialization
- Windows Forms (for folder browser dialog)

## License

MIT License
