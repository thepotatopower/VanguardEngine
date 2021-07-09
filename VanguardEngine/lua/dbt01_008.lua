-- Hexaorb Sorceress

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 7
end

function GetParam(n)
	if n == 1 then 
		return q.Location, l.RevealedTriggers, q.UnitType, u.Trigger, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Count, 1
	elseif n == 3 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 4 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 5 then
		return q.Location, l.PlayerHand, q.Trigger, tt.Critical, q.Trigger, tt.Front, q.Count, 1
	elseif n == 6 then
		return q.Location, l.Revealed
	elseif n == 7 then
		return q.Location, l.PlayerVC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnDriveCheck, t.Auto, p.HasPrompt, true, p.IsMandatory, true
	elseif n == 2 then
		return a.OnACT, t.ACT, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() and obj.Exists(1) and obj.Exists(2) then
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() and not obj.Activated() and obj.PersonaRode() and obj.CanCB(3) and obj.CanSB(4) and obj.Exists(5) then
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
	end
	return false
end

function Cost(n)
	if n == 2 then
		obj.CounterBlast(3)
		obj.SoulBlast(4)
	end
end

function Activate(n)
	if n == 1 then
		obj.ChooseAddTempPower(2, 10000)
	elseif n == 2 then
		obj.ChooseReveal(5)
		obj.SendToTop(6)
		obj.AddDrive(7, 1)
		obj.EndReveal()
	end
	return 0
end