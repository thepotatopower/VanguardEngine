-- 封焔の盾 スワヤンブー

function RegisterAbilities()
	-- on order
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("OnOrderCost")
	ability1.SetCanFullyResolve("OnOrderCanFullyResolve")
	ability1.SetActivation("OnOrder")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(1)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.VC)
	ability2.SetProperty(p.OncePerTurn)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCondition("OnAttackCondition")
	ability2.SetActivation("OnAttack")
	ability2.SetProperty(p.NotMandatory)
end

function OnOrderCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function OnOrderCanFullyResolve()
	return obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dsd06_001")})
end

function OnOrder()
	obj.Arm({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dsd06_001")}, false)
end

function OnAttackTrigger()
	if obj.Exists({q.Location, l.MyArmedUnit, q.Other, o.Attacked}) then
		obj.Track({q.Location, l.MyArmedUnit, q.Other, o.Attacked})
		return true
	end
	return false
end

function OnAttackCondition()
	return obj.Exists({q.Location, l.Tracking, q.Other, o.SameZone})
end

function OnAttack()
	obj.AddCardValue({q.Location, l.Tracking, q.Other, o.SameZone}, cs.BonusPower, 10000, p.UntilEndOfBattle)
end