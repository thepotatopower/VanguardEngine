-- すてぷみゃー

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBattleEnds)
	ability1.SetLocation(l.BackRowRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsPlayerAction() and obj.Exists({q.Location, l.RevealedTriggers, q.UnitType, u.Trigger})
end

function Cost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function CanFullyResolve()
	return obj.IsSameZone()
end

function Activation()
	obj.Stand({q.Other, o.ThisFieldID})
	obj.ChooseMoveRearguard({q.Other, o.ThisFieldID}, {FL.BackRow, FL.OpenCircle})
end