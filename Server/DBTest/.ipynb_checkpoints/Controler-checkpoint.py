import sqlite3
import random

# 连接到 SQLite 数据库
conn = sqlite3.connect(r'D:\UProjes\Assets\StreamingAssets\SQLiteData.db')
cursor = conn.cursor()

# 随机选择15个产品
# 1. 获取所有 goodsNo
cursor.execute('SELECT goodsNo FROM Goods')
all_goods = cursor.fetchall()
goods_nos = [row[0] for row in all_goods]

# 2. 随机选择15个产品
selected_goods_nos = random.sample(goods_nos, min(15, len(goods_nos)))

# 3. 执行联表查询
placeholders = ', '.join('?' * len(selected_goods_nos))
query = f'''
SELECT 
    g.goodsNo, 
    g.quality, 
    g.trait, 
    p.price, 
    p.stock 
FROM 
    Goods g
JOIN 
    ProductDetails p
ON 
    g.goodsNo = p.goodsNo
WHERE 
    g.goodsNo IN ({placeholders})
'''

cursor.execute(query, selected_goods_nos)
results = cursor.fetchall()

# 打印结果
for row in results:
    print(f'goodsNo: {row[0]}, quality: {row[1]}, trait: {row[2]}, price: {row[3]}, stock: {row[4]}')

# 关闭数据库连接
conn.close()