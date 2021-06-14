-- Blaze Maiden, Reiyu

function NumberOfAbilities()
	return 3
end

function NumberOfParams(n)
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Deck, q.Name, "Vairina", q.Count, 1
	elseif n == 2 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerVC, q.Location, l.PlayerRC, q.Other, o.This
	end
end


function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, false, false
	elseif n == 2 then
		return a.OnAttack, true, true
	elseif n == 3 then
		return a.OnBattleEnds, true, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRodeUponThisTurn() and obj.VanguardIs("Chakrabarthi Divine Dragon, Nirvana") and obj.CanSB(2) and obj.CanSearch(1) then
			return true
		end
	elseif n == 2 then
		if obj.IsAttackingUnit() then
			return true
		end
	elseif n == 3 then
		if obj.IsAttackingUnit() then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.SoulBlast(2)
	end
end

function Activate(n)
	if n == 1 then
		obj.Search(1)
		obj.OnRideAbilityResolved()
	elseif n == 2 then
		obj.AddPower(3, 2000)
	elseif n == 3 then
		obj.AddPower(3, -2000)
	end
	return 0
end