# AudioLevelMeter

[Assets/Scripts/AudioLevelMeter](Assets/Scripts/AudioLevelMeter)

- ILevelMeter.cs : レベルメーターの共通化のためのインターフェース
- Ratio.cs : 有理数
- Time.cs : オーディオクロックや秒などの異なる倍率で表現された時間を統一的に扱う型
- PeakMeter.cs : ピークメーター
- AudioLevelMeter.cs : オーディオレベルメーター(ピークメーター含む)のUnityのためのインターフェース

テスト用音源は含まれていない．AudioSourceに適当な音源をアタッチして実行すればピークメーターのテストが実行可能．
