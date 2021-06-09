-- Blaze Maiden, Rino

function NumberOfAbilities()
	return 3
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Deck, q.Name, "Trickstar"
	elseif n == 2 then
		return q.Location, l.PlayerVC, q.Location, l.PlayerRC, q.This
	end
end


function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, l.TopSoul, false, false
	elseif n == 2 then
		return a.OnAttack, l.PlayerVC, l.PlayerRC, true, true
	elseif n == 3 then
		return a.OnBattleEnds, l.PlayerVC, l.PlayerRC, true, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRodeUponThisTurn() and obj.VanguardIs("Blaze Maiden, Reiyu") and obj.CanSuperiorCall(1) then
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

function Activate(n)
	if n == 1 then
		obj.SuperiorCall(1)
		obj.OnRideAbilityResolved()
	elseif n == 2 then
		obj.AddPower(2, 2000)
	elseif n == 3 then
		obj.AddPower(2, -2000)
	end
	return 0
end