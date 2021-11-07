_Changes for v 12.5_:
- Implemented ability to save specified deployment flags (creating shortcuts, kill process, etc.);
- Implemented package flags that allow to properly update deployed versions in registry;
- Some interface adjustments applied;
- App now able to add versions of successfully deployed packages to the registry. It allows you to skip deployment package in the next DPModule start without starting the deployed project (was required in previous version). Also, this option allows you to track versions of those projects that cannot check them on their own (KeyboardSwitcher, ESHQ, etc.);
- DPModule now able to filter packages with “only new” and “only updates” flags;
- Updated internal mechanism of checking for updates (applied compatibility with new GitHub theme)
