-- 守るべき誇り

function RegisterAbilities()
	-- on order
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("OrderCost")
	ability1.SetActivation("OrderActivation")
	-- cont
	local ability2 = NewAbility(GetID())
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.Order)
	ability2.SetActivation("Cont")
end

function OrderCost(check) 
	if check then return obj.CanDiscard(1) end
	obj.Discard(1)
end

function OrderActivation()
end

function Cont()
	obj.AddCardValue({q.Location, l.PlayerRC, q.Grade, 3}, cs.BonusSkills, s.Boost, p.Continuous)
end