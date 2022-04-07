-- アグレスブルー・ドラゴン

function RegisterAbilities()
	-- on end of battle
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBattleEnds)
	ability1.SetLocation(l.PlayerRC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit() and obj.AttackedRearguard()
end

function Condition()
	return obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt03_009")})
end

function CanFullyResolve()
	return obj.IsSameZone() and obj.Exists({q.Location, l.PlayerUnits, q.Other, o.This, q.Other, o.Resting})
end

function Cost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function Activation()
	if obj.IsSameZone() then
		obj.Stand({q.Location, l.PlayerUnits, q.Other, o.This})
	end
end