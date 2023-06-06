# AutoDrop

For Oxide / Umod Rust Servers - A configurable plugin for automatically dropping items from a player's inventory or on pickup in Rust.

## Features

- Automatically drop items from a player's inventory when they are added or picked up.
- Configurable filter based on item shortnames.
- Permission system to control access to the plugin's commands.

## Installation

1. Download the `AutoDrop.cs` file from this repository.
2. Place the file in your Rust server's `oxide/plugins` directory.
3. Start or restart your Rust server.
4. The plugin will be automatically loaded and initialized.

## Usage

The plugin provides the following command:

- `/autodrop <itemShortname1> <itemShortname2> ...`: Register item shortnames for automatic drop. Players with the appropriate permission can use this command to specify the item shortnames they want to automatically drop from their inventory or when picked up. Each item shortname should be separated by a space.

## Permissions

- `autodrop.use`: Allows players to use the `/autodrop` command and register item shortnames for automatic drop.

## Configuration

The plugin does not have a configuration file. However, the registered item shortnames for automatic drop are stored in the `AutoDropItems` data file.

## Support

If you encounter any issues or have any questions or suggestions, please create an issue in the [GitHub repository](https://github.com/your/repository).

## License

This plugin is licensed under the [MIT License](LICENSE).
