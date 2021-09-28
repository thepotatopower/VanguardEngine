-- Archangel of Twin Wings, Alestiel

function NumberOfAbilities()
	return 3
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Bind, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerVC, q.Other, o.This
	elseif n == 3 then
		return q.Location, l.EnemyUnits
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnMainPhase, p.HasPrompt, p.IsMandatory
	elseif n == 2 then
		return a.Cont, p.IsMandatory
	elseif n == 3 then
		return a.Cont, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() and obj.IsPlayerTurn() then
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() and obj.IsWhiteWings() then
			return true
		end
	elseif n == 3 then
		if obj.IsVanguard() and obj.IsBlackWings() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n) 
	if n == 1 then
		return true
	elseif n == 2 then
		return true
	elseif n == 3 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		if obj.ChooseAddToHand(1) then
			obj.BindTopOfDeck(1)
		end
	elseif n == 2 then
		if obj.IsPlayerTurn() then
			obj.SetAbilityPower(2, 5000)
			obj.SetAbilityCritical(2, 1)
		end
	elseif n == 3 then
		if obj.IsPlayerTurn() then
			obj.SetAbilityPower(3, -5000)
		end
	end
	return 0
end