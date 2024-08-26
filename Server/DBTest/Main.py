import sqlite3
import random
import string

# 创建并连接数据库
conn = sqlite3.connect('D:\\UProjes\\Assets\\StreamingAssets\\SQLiteData.db')
cursor = conn.cursor()

# 获取刚插入 Goods 表的所有 goodsNo
cursor.execute('SELECT goodsNo FROM Goods')
goods_nos = cursor.fetchall()

# 随机填充 ProductDetails 表
for goods_no in goods_nos:
    for _ in range(random.randint(1, 50)):
        price = random.randint(500, 10000)  # 价格在500到10000之间，单位为美分
        stock = random.randint(1, 100)  # 生成1到100之间的整数
        cursor.execute('''
        INSERT INTO ProductDetails (goodsNo, price, stock)
        VALUES (?, ?, ?)
        ON CONFLICT(goodsNo, price)
        DO UPDATE SET stock = stock + excluded.stock;  -- 将现有 stock 值与新值相加
        ''', (goods_no[0], price, stock))

# 提交更改并关闭数据库连接
conn.commit()
conn.close()