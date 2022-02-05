_Changes for v 13.1.37_:
- Implemented downloading updates directly from the “About the app” interface (for other projects). This function requires DPModule. It allows you to download DPModule if it was not previously installed;
- Removed self-update function from the “About the app” interface;
- Upgraded Policy and Version info loaders;
- Implemented the fast deployment mode. It allows you to skip manual deployment steps and use previously specified path and flags by default (or set default values for new packages);
- Added new mechanism of access rights checking: it will display warnings when Windows registry is unavailable
