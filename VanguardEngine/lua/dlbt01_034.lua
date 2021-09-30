-- Shining As-is, Alestiel

function NumberOfAbilities()
	return 3
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Bind, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerVC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnVC, p.HasPrompt, p.IsMandatory
	elseif n == 2 then
		return a.OnACT, p.HasPrompt, p.OncePerTurn, p.WhiteWings, p.CB, 1
	elseif n == 3 then
		return a.Cont, p.IsMandatory, p.BlackWings
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnVC() then
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() then
			return true
		end
	elseif n == 3 then
		if obj.IsVanguard() then
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
		if obj.ChooseSendToBottom(1) then
			obj.BindTopOfDeck(1)
		end
	elseif n == 2 then
		obj.AddCritical(2, 1)
	elseif n == 3 then
		if obj.IsAttackingUnit() then
			obj.AddContinuousEnemyValue(ps.CannotGuardWithUnitType, u.Trigger)
		end
	end
	return 0
end