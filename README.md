# TicTacToe

Tic Tac Toe Game in C#

[https://hoangphongdhhp.blogspot.com/2016/07/game-co-caro-viet-bang-c.html](https://hoangphongdhhp.blogspot.com/2016/07/game-co-caro-viet-bang-c.html)

**Thuật toán MinMax áp dụng trò chơi cờ caro**

Bàn cờ bao gồm các ô cờ. Khi người chơi đánh vào 1 ô cờ máy sẽ thực hiện:

- Xét toàn bộ ô cờ trong bàn cờ và xem xét những ô cờ chưa có ai đánh và ô cờ đó có 'ý nghĩa' (cắt tỉa Alpha beta)

- Tìm giá trị Max. Max ở đây là giá trị lớn hơn giữa điểm tấn công và điểm phòng ngự

- Sau khi xác định được giá trị Max, máy sẽ đánh vào vị trí ô cờ có giá trị Max đó

## Cấu trúc dữ liệu và thuật toán cài đặt thực tế

### Cấu trúc dữ liệu:

+ Ô cờ: 1 class gồm thuộc tính vị trí dòng, vị trí cột, và sở hữu vị trí dòng để xác định vị trí cửa ô cờ trên bàn cờ

+ Bàn cờ: mảng hai chiều các ô cờ

### Giải thuật:

+ Mặc định khi ô cờ lúc khởi tạo ô cờ ( chưa ai đánh ) sẽ có sở hữu là 0

+ Khi người chơi đánh vào ô cờ thì sở hữu sẽ là 1

+ Khi máy đánh vào ô cờ sở hữu sẽ là 2

+ Dựa vào đó khi xét ô cờ cho máy đánh, sẽ xét những ô cờ có sở hữu là 0 (chưa có ai đánh) và ô cờ đó không bị cắt tỉa

### Tìm ô cờ có điểm tối ưu nhất cả về tấn công và phòng ngự trong bàn cờ:

+ Tính điểm tấn công: xét theo 4 hướng(ngang, dọc, chéo trên, chéo dưới) xét những quân cờ có sở hữu là 2(quân cờ của máy) và sẽ tăng điểm tấn công lên

+ Tính điểm phòng ngự: xét theo 4 hướng(ngang, dọc, chéo trên, chéo dưới) xét những quân cờ có sở hữu là 1(quân cờ của người chơi) sẽ tăng điểm phòng ngự lên

+ So sánh giữa điểm tấn công và phòng ngự điểm nào lớn hơn thì sẽ chọn làm điểm đánh cờ
