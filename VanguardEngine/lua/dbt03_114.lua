-- 樹角獣 モールモーラ

function RegisterAbilities()
	-- on attack
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.Exists({q.Location, l.BackRowRC, q.Other, o.Attacking, q.Other, o.NotThis}) 
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.PlayerUnits, q.Other, o.Attacking}) 
end

function Cost(check)
	if check then return obj.CanRest({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}) end
	obj.Rest({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID})
end

function Activation()
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.Attacking}, cs.BonusPower, 10000, p.UntilEndOfBattle)
end