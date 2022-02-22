-- 急転直下のギアイーグル

function RegisterAbilities()
	-- on attack hits
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttackHits)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsBooster()
end

function Cost(check) 
	if check then return obj.CanRetire({q.Other, o.ThisFieldID}) end
	obj.Retire({q.Other, o.ThisFieldID})
end

function CanFullyResolve()
	return obj.CanSuperiorCall({q.Location, l.Soul, q.Grade, 1, q.Count, 1})
end

function Activation()
	obj.SuperiorCall({q.Location, l.Soul, q.Grade, 1, q.Count, 1})
end