FROM ubuntu:20.04

ENV ANDROID_SDK_ROOT=/opt/android-sdk
ENV DEBIAN_FRONTEND=noninteractive

# ����� ����
RUN apt-get update && apt-get install -y \
    openjdk-11-jdk curl unzip git android-tools-adb \
    libglu1-mesa mesa-utils xvfb x11vnc fluxbox net-tools \
    && rm -rf /var/lib/apt/lists/*

# ����� ������ �����
RUN mkdir -p $ANDROID_SDK_ROOT/cmdline-tools

# ���� �������
WORKDIR $ANDROID_SDK_ROOT/cmdline-tools

# ����� command line tools
RUN curl -o sdk.zip https://dl.google.com/android/repository/commandlinetools-linux-8512546_latest.zip \
    && unzip sdk.zip -d . && rm sdk.zip \
    && mv cmdline-tools latest

# ����� sdkmanager �-PATH
ENV PATH=$ANDROID_SDK_ROOT/cmdline-tools/latest/bin:$PATH

# �� ����� sdkmanager ���� - ���� ����
RUN yes | sdkmanager --licenses && \
    sdkmanager \
        "platform-tools" \
        "emulator" \
        "system-images;android-30;google_apis;arm64-v8a" \
        "platforms;android-30"

COPY entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

ENTRYPOINT ["/entrypoint.sh"]