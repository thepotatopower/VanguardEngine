-- 砂塵の鋼弾 イーサン

function RegisterAbilities()
	-- on attack hits
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttackHits)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.Exists({q.Location, l.PlayerVC, q.Other, o.Attacking})
end

function Cost(check)
	if check then return obj.CanRetire({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}) end
	obj.Retire({q.Location, l.PlayerUnits, q.Other, o.This})
end

function Activation()
	obj.Draw(1)
	obj.CounterCharge(1)
end