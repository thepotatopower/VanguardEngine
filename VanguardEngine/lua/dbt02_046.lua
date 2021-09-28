-- Blaster Dark

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.NotThis, q.Count, 1
	elseif n == 2 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnVC, a.PlacedOnRC, p.HasPrompt, p.CB, 1, p.Retire, 1
	elseif n == 2 then
		return a.Cont, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if (obj.CurrentActivationIs(a.PlacedOnVC) and obj.LastPlacedOnVC()) or (obj.CurrentActivationIs(a.PlacedOnRC) and obj.LastPlacedOnRC()) then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.CanRetire(2) then
			return true
		end
	elseif n == 2 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.ChooseRetire(2) 
		obj.AddDrive(3, 1)
	elseif n == 2 then
		if obj.IsPlayerTurn() and obj.PlayerRetiredThisTurn() then
			obj.SetAbilityPower(3, 5000)
		end
	end
	return 0
end
