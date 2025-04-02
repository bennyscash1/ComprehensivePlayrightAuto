FROM node:18-slim

# התקנות בסיסיות כולל adb
RUN apt-get update && apt-get install -y \
    android-tools-adb \
    openjdk-17-jdk \
    curl \
    unzip \
 && apt-get clean \
 && rm -rf /var/lib/apt/lists/*

# התקנת Appium
RUN npm install -g appium@2.11.3 \
 && appium driver install uiautomator2

# יצירת תיקיית עבודה
WORKDIR /app

# פתיחת פורט ל-Appium
EXPOSE 4723

# Healthcheck לוודא שהשרת רץ
HEALTHCHECK --interval=30s --timeout=30s --start-period=30s --retries=3 \
 CMD curl -f http://localhost:4723/wd/hub/status || exit 1
