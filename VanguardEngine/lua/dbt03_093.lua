-- リファブリッシュメント・ドック

function RegisterAbilities()
	-- order
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("OrderCost")
	ability1.SetActivation("Order")
	-- on put
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(1)
	ability2.SetTiming(a.OnPut)
	ability2.SetMovedTo(l.Order, l.Player)
	ability2.SetTrigger("OnPutTrigger")
	ability2.SetActivation("OnPut")
	-- cont
	local ability3 = NewAbility(GetID())
	ability3.SetDescription(2)
	ability3.SetTiming(a.Cont)
	ability3.SetLocation(l.Order)
	ability3.SetActivation("Cont")
end

function OrderCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function Order()
end

function OnPutTrigger()
	return obj.IsApplicable()
end

function OnPut()
	obj.Draw(1)
end

function Cont()
	obj.AddCardValue({q.Location, l.PlayerRC, q.Grade, 2}, cs.BonusSkills, s.Boost, p.Continuous)
end