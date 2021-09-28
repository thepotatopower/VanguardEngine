-- Indecisive Sky, Alestiel

function NumberOfAbilities()
	return 3
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerVC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.EnemyRC
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnVC, p.HasPrompt, p.IsMandatory
	elseif n == 2 then
		return a.Cont, p.IsMandatory
	elseif n == 3 then
		return a.Cont, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnVC() then
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
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.BindTopOfDeck(1)
	elseif n == 2 then
		obj.AddContinuousPlayerState(ps.SoulBlastForRideDeck)
	elseif n == 3 then
		obj.AddContinuousValue(2, cs.CannotAttackUnit, 1)
	end
	return 0
end