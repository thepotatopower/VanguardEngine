-- 柩機の姫 ラクタァト

function RegisterAbilities()
	-- on end of battle
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBattleEnds)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsBooster() and obj.Exists({q.Location, l.PlayerUnits, q.Name, obj.GetNameFromCardID("dbt01_t01"), q.UnitType, u.Token, q.Other, o.Attacking})
end

function Condition()
	return obj.IsSameZone()
end

function Activation()
	local circle = obj.GetCircle({q.Other, o.ThisFieldID})
	obj.SuperiorCall({}, false, "dbt01_t01", false, {circle})
end
