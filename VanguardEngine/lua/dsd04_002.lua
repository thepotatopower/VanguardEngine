-- Sylvan Horned Beast, Lattice

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Revealed, q.Other, o.Unit
	elseif n == 3 then
		return q.Location, l.Revealed
	elseif n == 4 then
		return q.Location, l.BackRow, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, false, false
	elseif n == 2 then
		return a.OnAttack, false, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRodeUponThisTurn() and obj.VanguardIs("Sylvan Horned Beast King, Magnolia") then
			return true
		end
	elseif n == 2 then
		if obj.IsAttackingUnit() and obj.TargetIsEnemyVanguard() and obj.CanSB(1) and obj.Exists(4) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.SoulBlast(1)
	elseif n == 2 then
		obj.SoulBlast(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.RevealFromDeck(1)
		if obj.Exists(2) then
			obj.SuperiorCall(2)
		else
			obj.AutoAddToHand(3)
		end
		obj.OnRideAbilityResolved()
	elseif n == 2 then
		obj.AddBattleOnlyPower(4, 10000)
	end
	return 0
end