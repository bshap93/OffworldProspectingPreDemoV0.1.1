/*
----------------------------------
Compass Navigator Pro Support for Quest Machine
Copyright © Pixel Crushers. All rights reserved.
Compass Navigator Pro copyright © Kronnect
----------------------------------

To enable Compass Navigator Pro 2 integration, you must import two packages:

- Plugins / Pixel Crushers / Common / Third Party Support / Compass Navigator Pro Support.unitypackage
- Plugins / Pixel Crushers / Quest Machine / Third Party Support / Compass Navigator Pro Support.unitypackage



INSTRUCTIONS:

- The Compass Pro POI Quest Action lets you control POIs in quests.

- The Quest Tracking POI component make a POI only visible when a specified quest
  (and optionally a specific quest node) is being tracked.

- Use a Compass Pro Event component to send messages using the Pixel Crushers
  Message System. The name of the POI is passed as the message's Value. The
  demo scene's quest listens for the message "Visited":"POI":(poi-name) to
  move each node of the quest forward.

- You can add a POI Saver component to a POI to save its state in saved games.
*/