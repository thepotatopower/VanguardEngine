-- Hexaorb Sorceress

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then 
		return q.Location, l.RevealedTrigger, q.UnitType, u.Trigger, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerHand, q.Trigger, tt.Critical, q.Trigger, tt.Front, q.Count, 1
	elseif n == 4 then
		return q.Location, l.Revealed
	elseif n == 5 then
		return q.Location, l.PlayerVC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnDriveCheck, p.HasPrompt, p.IsMandatory
	elseif n == 2 then
		return a.OnACT, p.HasPrompt, p.OncePerTurn, p.CB, 1, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() and obj.IsPlayerTurn() and obj.Exists(1) and obj.Exists(2) then
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() and not obj.Activated() and obj.PersonaRode() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	elseif n == 2 then
		if obj.Exists(3) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.ChooseAddTempPower(2, 10000)
	elseif n == 2 then
		if obj.Exists(3) then
			obj.ChooseReveal(3)
			obj.SendToTop(4)
			obj.AddDrive(5, 1)
			obj.EndReveal()
		end
	end
	return 0
end