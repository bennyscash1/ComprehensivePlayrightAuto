FROM node:18-slim

# ������ ������� ���� adb
RUN apt-get update && apt-get install -y \
    android-tools-adb \
    openjdk-17-jdk \
    curl \
    unzip \
 && apt-get clean \
 && rm -rf /var/lib/apt/lists/*

# ����� Appium
RUN npm install -g appium@2.11.3 \
 && appium driver install uiautomator2

# ����� ������ �����
WORKDIR /app

# ����� ���� �-Appium
EXPOSE 4723

# Healthcheck ����� ����� ��
HEALTHCHECK --interval=30s --timeout=30s --start-period=30s --retries=3 \
 CMD curl -f http://localhost:4723/wd/hub/status || exit 1
