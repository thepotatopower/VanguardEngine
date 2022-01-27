-- 再来の魔法 ララリタ

function RegisterAbilities()
	-- on battle ends
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBattleEnds)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.Exists({q.Location, l.RevealedTriggers})
end

function Condition()
	return obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt01_008")})
end

function Cost(check)
	if check then return obj.CanCB(1) and obj.CanSB(1) end
	obj.CounterBlast(1)
	obj.SoulBlast(1)
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.PlayerRC, q.Other, o.NotThisFieldID, q.Other, o.Resting})
end

function Activation()
	obj.Select({q.Location, l.PlayerRC, q.Other, o.NotThisFieldID, q.Count, 1})
	obj.Stand({q.Location, l.Selected})
	obj.Retire({q.Other, o.ThisFieldID})
end