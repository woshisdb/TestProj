import numpy as np
from scipy.stats import zscore
from typing import List, Dict
import sqlite3
import random
from dataclasses import dataclass
# 定义数据类
@dataclass
class Product:
    goodsNo: int
    quality: float
    trait: float
    price: int
    stock: int
# 连接到 SQLite 数据库
conn = sqlite3.connect(r'..\..\Assets\StreamingAssets\SQLiteData.db')
cursor = conn.cursor()

money = 700
print(money)
# 3. 执行联表查询
#placeholders = ', '.join('?' * len(selected_goods_nos))
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
    p.price < {money}
ORDER BY RANDOM() 
LIMIT 10
'''

cursor.execute(query)
results = cursor.fetchall()

products = [Product(goodsNo=row[0], quality=row[1], trait=row[2], price=row[3], stock=row[4]) for row in results]
# 打印结果
for product in products:
    print(product)

def calculate_weighted_score(products: List[Product], weights: Dict[str, float]) -> List[float]:
    # 转换产品属性为数组以便于计算
    qualities = np.array([p.quality for p in products])
    traits = np.array([p.trait for p in products])
    prices = np.array([p.price for p in products])
    stocks = np.array([p.stock for p in products])

    # 进行 Z-score 归一化
    z_qualities = zscore(qualities)
    z_traits = zscore(traits)
    z_prices = zscore(money-prices)

    # 计算加权平均值
    weighted_scores = []
    for i in range(len(products)):
        score = (z_qualities[i] * weights['quality'] +
                 z_traits[i] * weights['trait'] +
                 z_prices[i] * weights['price'])
        weighted_scores.append(score)

    return weighted_scores
weights = {
    'quality': 0.3,
    'trait': 0.2,
    'price': 0.4,
}
weighted_scores = calculate_weighted_score(products, weights)
for product, score in zip(products, weighted_scores):
    print(f'Product {product.goodsNo} - Weighted Score: {score:.2f}')

# 关闭数据库连接
conn.close()

class Need:
    """
    表示需求的基类
    """
    def __init__(self, rate):
        """
        初始化需求对象
        :param rate: 需求所占的比例
        """
        self.rate = rate

class Alive(Need):
    """
    对活着的需求
    """

    def __init__(self, rate):
        super().__init__(rate)

class Safety(Need):
    """
    对安全的需求
    """
    def __init__(self, rate):
        super().__init__(rate)

class Belong(Need):
    """
    对归属的需求
    """
    def __init__(self, rate):
        super().__init__(rate)

class Esteem(Need):
    """
    对尊重的需求
    """
    def __init__(self, rate):
        super().__init__(rate)

class NPC:
    def __init__(self):
        self.alive=Alive(rate=0.8)
        self.safety=Safety(rate=0.9)
        self.belong=Belong(rate=0.7)
        self.esteem=Esteem(rate=0.6)
    #
    def exec
    #重新计算合适的比例
    def replan(self):
